myApp
    .factory('transferRequisitionFinalizeCancelService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToCancelRequisitionFinalize = function (id, reason) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TU0403'
                        + '?id=' + id
                        + '&reason=' + reason
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);