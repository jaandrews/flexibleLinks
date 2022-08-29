(function () {
    angular.module('umbraco.filters')
        .filter('flLabels', flLabels)

    function flLabels() {
        return function (input) {
            if (!input) {
                return '';
            }
            var result = "";
            input.forEach(link => {
                if (result) {
                    result += ', '
                }
                result += link.label
            });
            return result;
        }
    }
})();