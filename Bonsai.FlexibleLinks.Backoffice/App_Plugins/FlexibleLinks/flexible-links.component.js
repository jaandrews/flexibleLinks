(function () {
    /**
     * @ngdoc component
     * @name flexibleLinks
     * @description
     * Displays a collection of links that can connect to content, media, modals, external urls and phones.
     * @param {object=} config Configuration settings for the control.
     * @param {object} model The data the control is connected to. Uses whatever expression is set in `ngModel.`
     * */
    angular.module('umbraco')
        .component('flexibleLinks', {
            bindings: {
                config: '<?',
                model: '<ngModel'
            },
            require: {
                ngModel: 'ngModel'
            },
            template: `
                <umb-load-indicator ng-if="$ctrl.loading"></umb-load-indicator>
                <div ng-if="!$ctrl.loading">
                    <ul class="links" ng-class="{'has-links': $ctrl.renderModel.length}" ui-sortable="$ctrl.sortableOptions" ng-model="$ctrl.renderModel">
                        <li ng-repeat="link in $ctrl.renderModel">
                            <flexible-link disable-label="$ctrl.disableLabel" link="link" on-change="$ctrl.updateModel(link, $index)" config="$ctrl.config" on-remove="$ctrl.removeLink($index)"></flexible-link>
                        </li>
                    </ul>
                    <a class="add" ng-click="$ctrl.openLinkOptions()" ng-if="$ctrl.allowMultiple || !$ctrl.renderModel.length"><span class="icon icon-add"></span></a>
                </div>
            `,
            controller: FlexibleLinksController
        });
    FlexibleLinksController.$inject = ['$q', 'contentResource', 'mediaResource', 'editorService', '$timeout', '$element', '$http', 'notificationsService'];
    function FlexibleLinksController($q, contentResource, mediaResource, editorService, $timeout, $element, $http, notificationsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.loading = true;
            ctrl.disableLabel = ctrl.config.disableLabels === '1';
            if (ctrl.disableLabel) {
                $element.addClass('disable-labels');
            }
            ctrl.allowMultiple = ctrl.config.allowMultiple === '1';
            ctrl._initRenderModel();
            ctrl.sortableOptions = {
                handle: '.type',
                axis: 'y',
                start: function (e, ui) {
                    ctrl._originalIndex = ui.item.index();
                },
                stop: function (e, ui) {
                    var newIndex = ui.item.index();
                    // Move the element in the model
                    if (ctrl._originalIndex !== newIndex) {
                        $timeout(function () {
                            var mover = ctrl.renderModel[ctrl._originalIndex];
                            var moved = ctrl.renderModel[newIndex];
                            ctrl.model[ctrl._originalIndex] = {
                                id: mover.id,
                                url: mover.url,
                                label: mover.label,
                                type: mover.type
                            };
                            ctrl.model[newIndex] = {
                                id: moved.id,
                                url: moved.url,
                                label: moved.label,
                                type: moved.type
                            };
                            ctrl._originalIndex = undefined;
                        });
                    }
                }
            };
            ctrl._configureAdder();
        };

        ctrl._initRenderModel = function () {
            ctrl.renderModel = [];
            if (ctrl.model && ctrl.model.length) {
                ctrl.renderModel = angular.copy(ctrl.model);
            }
            ctrl.loading = false;
        };

        ctrl.removeLink = function (index) {
            ctrl.renderModel.splice(index, 1);
            ctrl.model.splice(index, 1);
        };

        ctrl.updateModel = function (link, index) {
            ctrl.model[index] = {
                id: link.id,
                label: link.label,
                url: link.url,
                type: link.type,
                newTab: link.newTab,
                extra: link.extra
            };
        };

        ctrl._configureAdder = function () {
            $http.get('/umbraco/backoffice/flexiblelinks/flexiblelinksapi/gettypes', {
                cache: true
            })
                .then(function (response) {
                    ctrl.addOptions = [];

                    response.data.forEach(type => {
                        if (ctrl.config.typeSettings[type.key] && ctrl.config.typeSettings[type.key].enabled) {
                            ctrl.addOptions.push(type);
                        }
                    });
                })
                .catch(function (error) {
                    notificationsService.error('Failed to load link types.');
                });
        };

        ctrl.openLinkOptions = function () {
            if (ctrl.addOptions.length > 1) {
                editorService.itemPicker({
                    title: "Choose Link Type",
                    availableItems: ctrl.addOptions,
                    filter: false,
                    submit: ctrl._handlePick,
                    close: function () {
                        editorService.close();
                    }
                });
            }
            else if (ctrl.addOptions.length === 1) {
                ctrl._handlePick({
                    selectedItem: ctrl.addOptions[0],
                    skippedOpening: true
                });
            }
        };

        ctrl._handlePick = function (response) {
            var selection = response.selectedItem;
            var isNode = selection.picker === 'Content' || selection.picker === 'Media';
            if (isNode) {
                var service = selection.picker === 'Media' ? 'mediaPicker' : 'contentPicker';
                var settings = {
                    multiPicker: ctrl.allowMultiple,
                    submit: function (innerResponse) {
                        var selections = innerResponse.selection;
                        if (selections.length) {
                            var ids = [];
                            for (var i = 0; i < selections.length; i++) {
                                var innerSelection = selections[i];
                                ids.push(innerSelection.id);
                                ctrl.renderModel.push({
                                    id: innerSelection.udi,
                                    label: ctrl.disableLabel ? null : innerSelection.name,
                                    type: selection.key,
                                    node: {
                                        name: innerSelection.name,
                                        icon: innerSelection.icon
                                    }
                                });
                                if (!ctrl.model) {
                                    ctrl.model = [];
                                }
                                ctrl.model.push({
                                    id: innerSelection.udi,
                                    label: ctrl.disableLabel ? null : innerSelection.name,
                                    type: selection.key
                                });
                            }
                            ctrl.ngModel.$setViewValue(ctrl.model);
                        }
                        if (!response.skippedOpening) {
                            editorService.close();
                        }
                        editorService.close();
                    },
                    close: function () {
                        editorService.close();
                    }
                };
                if (ctrl.config && ctrl.config.typeSettings && isNode) {
                    if (isNode) {
                        settings.startNodeId = ctrl.config.typeSettings[selection.key].settings.startNode || -1;
                    }
                }
                editorService[service](settings);
            }
            else if (selection.picker === 'Custom') {
                editorService.open({
                    view: selection.pickerPath,
                    size: 'small',
                    type: selection.key,
                    multiPicker: ctrl.allowMultiple,
                    submit: function (innerResponse) {
                        var selections = innerResponse.selection;
                        if (selections.length) {
                            var ids = [];
                            for (var i = 0; i < selections.length; i++) {
                                var innerSelection = selections[i];
                                ids.push(innerSelection.id);
                                ctrl.renderModel.push({
                                    id: innerSelection.id,
                                    label: ctrl.disableLabel ? null : innerSelection.name,
                                    type: selection.key,
                                    node: {
                                        name: innerSelection.name,
                                        icon: innerSelection.icon
                                    }
                                });
                                if (!ctrl.model) {
                                    ctrl.model = [];
                                }
                                ctrl.model.push({
                                    id: innerSelection.id,
                                    label: ctrl.disableLabel ? null : innerSelection.name,
                                    type: selection.key
                                });
                            }
                            ctrl.ngModel.$setViewValue(ctrl.model);
                        }
                        if (!response.skippedOpening) {
                            editorService.close();
                        }
                        editorService.close();
                    },
                    close: function () {
                        editorService.close();
                    }
                });
            }
            else {
                if (!ctrl.model) {
                    ctrl.model = [];
                }
                ctrl.renderModel.push({
                    url: null,
                    type: selection.key
                });
                ctrl.model.push({
                    type: selection.key
                });
                ctrl.ngModel.$setViewValue(ctrl.model);
                editorService.close();
            }
        }
    }
})();