(function () {
    angular.module('umbraco')
        .controller('FlexibleLinks.LinkTypesController', LinkTypesController)

    LinkTypesController.$inject = ['$scope', '$http', 'notificationsService'];
    function LinkTypesController($scope, $http, notificationsService) {
        var vm = this;

        vm.$onInit = function () {
            $scope.model.value = $scope.model.value || {};
            $http.get('/umbraco/backoffice/flexiblelinks/flexiblelinksapi/gettypes')
                .then(function (response) {
                    vm.types = response.data;
                    vm.types.forEach(type => {
                        $scope.model.value[type.key] = $scope.model.value[type.key] || {
                            enabled: false,
                            settings: {}
                        };
                        if (type.settings && $scope.model.value[type.key].settings) {
                            type.settings.forEach(setting => {
                                if ($scope.model.value[type.key].settings[setting.key]) {
                                    setting.value = $scope.model.value[type.key].settings[setting.key];
                                }
                            });
                        }
                    });
                    $scope.$watch('vm.types', function (types) {
                        types.forEach(type => {
                            if (type.settings) {
                                type.settings.forEach(setting => {
                                    $scope.model.value[type.key].settings[setting.key] = setting.value;
                                })
                            }
                        })
                    }, true)
                })
                .catch(function (error) {
                    notificationsService.error('Failed to load link types.');
                });
        }

        vm.toggle = function (key) {
            $scope.model.value[key].enabled = !$scope.model.value[key].enabled;
        }
    }
})();