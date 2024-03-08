myApp
    .factory('shipmentModeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchShipmentMode = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0047'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);