myApp
    .factory('databaseUpdateService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.UpdateFullDatabase = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/UpdateDatabase'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);