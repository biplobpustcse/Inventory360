myApp
    .factory('allCollectionService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllCollectionLists = function (q, currency, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0023'
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