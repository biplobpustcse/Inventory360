myApp
    .factory('customerDeliveryCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchDeliveryNumber = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00234'
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