myApp
    .factory('accountsHeadCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllAccountsHead = function (q, oppositeId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0009'
                        + '?query=' + q
                        + '&oppositeId=' + oppositeId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);