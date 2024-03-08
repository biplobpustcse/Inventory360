myApp
    .factory('currencyCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchCurrency = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0050'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchCurrencyRate = function (currency) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0051'
                        + '?currency=' + currency
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);