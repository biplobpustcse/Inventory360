myApp
    .factory('transferOrderApproveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToApproveTransferOrder = function (id) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TU0408' 
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);