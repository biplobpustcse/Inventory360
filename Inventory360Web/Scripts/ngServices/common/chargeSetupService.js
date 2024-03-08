myApp
    .factory('chargeSetupService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};
            fac.FetchAllCharge = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S203'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchRMAWiseAllCharge = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S203E'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);