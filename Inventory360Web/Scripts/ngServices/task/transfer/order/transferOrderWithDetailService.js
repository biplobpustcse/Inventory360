myApp
    .factory('transferOrderWithDetailService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchSelectedTransferOrderDetail = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0410'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                });
            }

            return fac;
        }
    ]);