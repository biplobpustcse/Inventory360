myApp
    .factory('unApprovedCollectionService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchUnApprovedCollectionLists = function (q, currency, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0022'
                        + '?query=' + q
                        + '&currency=' + currency
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                    //+ '#' + Math.random().toString(36)
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);