myApp
    .factory('priceTypeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchPriceType = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0055'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchPriceTypeIsDetail = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0056'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);