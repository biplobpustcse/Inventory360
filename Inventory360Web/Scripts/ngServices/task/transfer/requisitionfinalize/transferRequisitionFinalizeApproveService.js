myApp
    .factory('transferRequisitionFinalizeApproveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToApproveRequisitionFinalize = function (id) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TU0402'
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