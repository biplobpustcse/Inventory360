myApp
    .factory('stockTypeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductStockType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0399'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);