myApp
    .factory('templateHeaderCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchTemplateHeader = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0041'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            return fac;
        }
    ]);