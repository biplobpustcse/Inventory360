myApp
    .factory('productTypeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0025'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);