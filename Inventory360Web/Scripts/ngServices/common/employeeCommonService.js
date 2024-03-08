myApp
    .factory('employeeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchEmployeeSalesPerson = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0023'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchAllEmployee = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0036'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);