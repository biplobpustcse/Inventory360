myApp
    .factory('allTransferRequisitionFinalizeService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllTransferRequisitionFinalize = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0405'
                        + '?query=' + q
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