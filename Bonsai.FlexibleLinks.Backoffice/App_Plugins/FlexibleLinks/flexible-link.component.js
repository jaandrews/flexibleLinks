(function () {
    /**
    * @ngdoc compoenent
    * @name flexibleLink
    * @description
    * Displays a link that can connect to content, media, modals, external urls and phones.
    * @param {object} link Data the flexible link renders.
    * @param {bool=} disableLable Removes the label control from the rendered view when true.
    * @param {object=} config Configuration settings for the rendered view.
    * @param {function=} onChange Callback function that triggers when the user makes a change to the label or url.
    * - link: current value of the control
    * @param {function=} onRemove Callback function that triggers when the flexible link is removed.
    * - link: current value of the control
    * */
    var id = 0;
    angular.module('umbraco')
        .component('flexibleLink', {
            bindings: {
                link: '<',
                disableLabel: '<?',
                config: '<?',
                onChange: '&',
                onRemove: '&'
            },
            template: `
                <div class="type">
                    <span class="icon" ng-class="$ctrl.icon"></span>
                </div>
                <div class="data">
                    <div class="label-container">
                        <div class="link-label">
                            <input type="text" ng-if="!$ctrl.disableLabel" ng-model="$ctrl.link.label" placeholder="Label" ng-change="$ctrl.updateModel()" />
                            <input ng-if="$ctrl.urlEditable && $ctrl.disableLabel" type="text" ng-model="$ctrl.link.url" placeholder="{{$ctrl._type.urlPlaceholder}}" ng-change="$ctrl.updateModel()" />
                            <span></span>
                            <p ng-if="$ctrl.disableLabel && !$ctrl.urlEditable">{{$ctrl.nodeUrl || 'Loading url...'}}</p>
                        </div>
                        <div class="link-label" ng-if="$ctrl.allowExtra">
                            <input type="text" ng-model="$ctrl.link.extra" placeholder="#value or ?key=value" ng-change="$ctrl.updateModel()" />
                            <span></span>
                        </div>
                        <div class="new-tab" ng-if="$ctrl._allowNewTab">
                            <input id="new-tab-{{$ctrl._id}}" type="checkbox" ng-model="$ctrl.link.newTab" ng-true-value="true" ng-false-value="false" ng-change="$ctrl.updateModel()">
                            <label for="new-tab-{{$ctrl._id}}">New Tab</label>
                        </div>
                    </div>
                    <div class="additional-info" ng-if="!$ctrl.disableLabel">
                        <input class="url" ng-if="$ctrl.urlEditable" type="text" ng-model="$ctrl.link.url" placeholder="{{$ctrl._type.urlPlaceholder}}" ng-change="$ctrl.updateModel()" />
                        <p ng-if="!$ctrl.urlEditable">{{$ctrl.nodeUrl || 'Loading url...'}}</p>
                    </div>
                </div>
                <div class="actions">
                    <a ng-if="$ctrl.isChangeable" ng-click="$ctrl.changeLink()"><span class="icon icon-info"></span></a>
                    <a ng-if="$ctrl.isChangeable" ng-click="$ctrl.setLink()"><span class="icon icon-edit"></span></a>
                    <a ng-click="$ctrl.removeLink()"><span class="icon icon-delete"></span></a>
                </div>
            `,
            controller: FlexibleLinkController
        });

    FlexibleLinkController.$inject = ['editorService', 'contentResource', 'mediaResource', '$element', 'entityResource', '$routeParams', 'entityResource', '$http', 'notificationsService'];
    function FlexibleLinkController(editorService, contentResource, mediaResource, $element, entityResource, $routeParams, entityResource, $http, notificationsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl._id = id++;
            if (ctrl.disableLabel) {
                $element.addClass('disabled-label');
            }
            ctrl._targetResource = contentResource;
            var culture = $routeParams.cculture || $routeParams.mculture;
            $http.get('/umbraco/backoffice/flexiblelinks/flexiblelinksapi/gettypes', {
                cache: true
            })
                .then(function (response) {
                    response.data.forEach(type => {
                        if (type.key === ctrl.link.type) {
                            ctrl._type = type;
                            ctrl.icon = type.icon;
                            ctrl.allowExtra = type.allowExtra;
                            ctrl._allowNewTab = type.allowNewTab;
                            ctrl.urlEditable = ctrl._type.picker === 'None' && ctrl._type.manual;
                            ctrl.isChangeable = ctrl._type.picker === 'Content' || ctrl._type.picker === 'Media';

                            if (ctrl.link.id) {
                                $http.get('/umbraco/backoffice/flexiblelinks/flexiblelinksapi/getinfo', {
                                    params: {
                                        type: ctrl.link.type,
                                        id: ctrl.link.id,
                                        culture: culture
                                    }
                                })
                                    .then(function (response) {
                                        ctrl.nodeUrl = response.data.url;
                                    })
                                    .catch(function (error) {
                                        ctrl.nodeUrl = 'Failed to find content.';
                                    });
                            }
                        }
                    });

                })
                .catch(function (error) {
                    notificationsService.error('Failed to load link types.');
                });
        };

        ctrl.updateModel = function () {
            if (ctrl.onChange) {
                ctrl.onChange({
                    link: ctrl.link
                });
            }
        };

        ctrl.changeLink = function () {
            switch (ctrl._type.picker) {
                case 'Content': case 'Media':
                    var service = ctrl._type.picker === 'Media' ? 'mediaEditor' : 'contentEditor';
                    var settings = {
                        id: ctrl.link.id,
                        submit: function (editor) {
                            editorService.close();
                        },
                        close: function () {
                            editorService.close();
                        }
                    };
                    if (ctrl.config) {
                        settings.startNodeId = ctrl.config.typeSettings[ctrl.link.type].startNode || -1;
                    }
                    editorService[service](settings);
                    break;
                case 3:
                    //editorService.open({

                    //});
                    break;

            }
        };

        ctrl.setLink = function () {
            var settings = {
                id: ctrl.link.id,
                submit: function (editor) {
                    var selection = editor.selection[0];

                    ctrl.link.id = selection.udi;
                    ctrl.updateModel();
                    entityResource.getUrl(selection.id, ctrl._type.type === 'Media' ? 'Media' : 'Document')
                        .then(function (url) {
                            ctrl.nodeUrl = url;
                            editorService.close();
                        })
                },
                close: function () {
                    editorService.close();
                }
            };
            if (ctrl.config) {
                settings.startNodeId = ctrl.config.typeSettings[ctrl.link.type].startNode || -1;
            }
            switch (ctrl._type.picker) {
                case 'Content': case 'Media':
                    editorService[ctrl._type.picker === 'Media' ? 'mediaPicker' : 'contentPicker'](settings);
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        };

        ctrl.removeLink = function () {
            if (this.onRemove) {
                ctrl.onRemove({
                    data: ctrl.link
                });
            }
        };
    }
})();