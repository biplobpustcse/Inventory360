myApp
    .factory('requisitionWithDetailService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchSelectedItemRequisitionDetail = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0025'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                });
            }

            fac.FetchItemRequisitionWithDetaillForRequisitionBy = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0028'
                        + '?query=' + q
                    //+ '#' + Math.random().toString(36)
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);