myApp
    .factory('securityUserChangePasswordService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ChangePassword = function (
                currentPassword, newPassword, confirmPassword
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/SEC02',
                    dataType: 'json',
                    headers: { "Content-Type": "application/json" },
                    data: JSON.stringify({
                        "CurrentPassword": currentPassword,
                        "NewPassword": newPassword,
                        "ConfirmPassword": confirmPassword
                    })
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);