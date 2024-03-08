myApp
    .factory('bankCommonService', [ '$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchBank = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0017'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            //Added By Biplob 14/03/2020
            fac.FetchOwnBank = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0080'
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