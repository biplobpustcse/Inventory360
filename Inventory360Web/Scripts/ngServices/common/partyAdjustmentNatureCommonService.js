myApp
    .factory('partyAdjustmentNatureCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchPartyAdjustmentNature = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0087'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ])