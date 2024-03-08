myApp
    .factory('transferRequisitionFinalizeWithDetailService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchSelectedFinalizeDetail = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0404'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                });
            }

            fac.FetchRequisitionFinalizeWithDetaill = function (query) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0407'
                        + '?query=' + query
                }).then(function (response) {
                    return response;
                });
            }

            return fac;
        }
    ]);