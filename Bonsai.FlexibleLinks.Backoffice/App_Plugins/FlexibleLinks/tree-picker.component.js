(function () {
    angular.module('umbraco')
        .component('treePicker', {
            bindings: {
                model: '<'
            },
            template: `
                <umb-editor-view>
                    <umb-editor-header name="vm.model.title"
                                       name-locked="true"
                                       hide-alias="true"
                                       hide-icon="true"
                                       hide-description="true">
                    </umb-editor-header>
                    <umb-editor-container>
                        <umb-box>
                            <umb-box-content>
                                <umb-load-indicator ng-if="vm.loading">
                                </umb-load-indicator>
                                <!-- FILTER -->
                                <div class="umb-control-group">
                                    <div class="form-search">
                                        <i class="icon-search"></i>
                                        <input type="text"
                                               class="-full-width-input"
                                               ng-model="vm.filterTerm"
                                               ng-model-options="{debounce: 800}"
                                               class="umb-search-field search-query search-input input-block-level"
                                               localize="placeholder"
                                               placeholder="@placeholders_filter"
                                               ng-change="vm.search()"
                                               umb-auto-focus>
                                    </div>
                                </div>
                                <div class="umb-tree">
                                    <tree-picker-item item="item" depth="1" ng-repeat="item in vm.items"></tree-picker-item>
                                </div>

                                <div ng-if="vm.error" class="alert alert-warning">
                                    {{ vm.error }}
                                </div>
                            </umb-box-content>
                        </umb-box>
                    </umb-editor-container>
                    <umb-editor-footer>
                        <umb-editor-footer-content-right>
                            <umb-button type="button"
                                        button-style="link"
                                        label-key="general_close"
                                        shortcut="esc"
                                        action="vm.close()">
                            </umb-button>
                            <umb-button type="button"
                                        button-style="action"
                                        label-key="general_submit"
                                        action="vm.submit()">
                            </umb-button>
                        </umb-editor-footer-content-right>
                    </umb-editor-footer>
                </umb-editor-view>
            `,
            controller: TreePickerController,
            controllerAs: 'vm'
        });

    TreePickerController.$inject = ['$http', '$routeParams'];
    function TreePickerController($http, $routeParams) {
        var vm = this;

        vm.loading = false;
        vm.error = null;

        vm.$onInit = function () {
            vm.loading = true;
            vm.model.selection = [];

            // set default title
            if (!vm.model.title) {
                vm.model.title = "Select option(s)";
            }

            // we don't need the submit button for a multi picker because we submit on select for the single picker
            if (!vm.model.multiPicker) {
                vm.model.hideSubmitButton = true;
            }

            // make sure we have an array to push to
            vm.model.selection = [];
            vm.getItems()
                .then(function (items) {
                    vm.items = items;
                    vm.loading = false;
                })
                .catch(function (response) {
                    vm.error = "An Error has occured while loading!";
                    vm.loading = false;
                });
        }

        vm.search = function () {
            vm.loading = true;

            if (vm.filterTerm) {
                $http.get('/umbraco/backoffice/flexiblelinks/flexiblelinksapi/search', {
                    params: {
                        searchTerm: vm.filterTerm,
                        type: vm.model.type,
                        culture: $routeParams.cculture || $routeParams.mculture
                    }
                })
                    .then(function (response) {
                        vm.items = response.data;
                        vm.loading = false;
                    });
            }
            else {
                vm.getItems()
                    .then(function (items) {
                        vm.items = items;
                        vm.loading = false;
                    })
            }
        }

        vm.getItems = function (id) {
            return $http.get('/umbraco/backoffice/flexiblelinks/flexiblelinksapi/gettree', {
                params: {
                    id: id || -1,
                    type: vm.model.type,
                    culture: $routeParams.cculture || $routeParams.mculture
                }
            })
                .then(function (response) {
                    return response.data;
                });
        }

        vm.close = function () {
            if (vm.model.close) {
                vm.model.close(vm.model);
            }
        }

        vm.submit = function () {
            if (vm.model.submit) {
                vm.model.submit(vm.model);
            }
        }

        vm.toggleItem = function (item) {
            var index = vm.model.selection.findIndex((selection) => {
                return selection.id === item.id;
            });
            if (index > -1) {
                vm.model.selection.splice(index, 1);
            } else {
                // store selected form in an array
                vm.model.selection.push(item);
                // if it's not a multipicker - submit the overlay
                if (!vm.model.multiPicker) {
                    vm.model.submit(vm.model);
                }

            }

        }
    }
})();