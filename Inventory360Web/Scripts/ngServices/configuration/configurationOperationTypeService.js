myApp
    .factory('configurationOperationTypeService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchOperationType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/CON001'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);