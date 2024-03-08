myApp
    .factory('transferOrderCancelService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToCancelTransferOrder = function (id, reason) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TU0409'
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