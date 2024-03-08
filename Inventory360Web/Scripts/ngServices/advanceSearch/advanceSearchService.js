myApp
    .factory('advanceSearchService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllAdvanceSearchDataLists = function (q, currency) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00241'
                        + '?serial=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);