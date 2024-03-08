myApp
    .factory('projectCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProjectByCompany = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0013'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);