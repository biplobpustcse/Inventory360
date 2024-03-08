myApp
    .factory('paymentModeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchPaymentMode = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0054'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);