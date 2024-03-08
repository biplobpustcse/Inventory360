myApp
    .controller('loginController', ['$scope', '$ngConfirm', 'applicationBasePath', 'accountService',
        function ($scope, $ngConfirm, applicationBasePath, accountService) {
            sessionStorage.clear();

            $scope.account = {
                username: '',
                password: ''
            };

            $scope.clickRefresh = function () {
                window.location.href = applicationBasePath;
            };

            $scope.login = function () {
                $scope.message = '';
                accountService.login($scope.account)
                    .then(function (data) {
                        window.location.href = applicationBasePath + "/Home";
                    }, function (error) {
                        // alert
                        $ngConfirm({
                            title: 'Warning',
                            icon: 'glyphicon glyphicon-exclamation-sign',
                            theme: 'supervan',
                            content: error.data.error_description,
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                    });
            };

            // load all parent location by selected company
            $scope.getLocationByCompanyForDropdown = function () {
                accountService.fetchLocationByCompany($scope.account.companyId).then(function (value) {
                    $scope.allLocationByCompanyJsonData = value.data;
                    if (value.data.length === 2) {
                        $scope.account.locationId = value.data[1].Value.toString();
                    }
                    else {
                        $.each(value.data, function () {
                            if (this.IsSelected) {
                                $scope.account.locationId = this.Value;
                            }
                        });
                    }
                }, function (reason) {
                    console.log(reason);
                });
            };

            // load all company
            var getAllCompanyForDropdown = function () {
                accountService.fetchAllCompany().then(function (value) {
                    $scope.allCompanyJsonData = value.data;
                    if (value.data.length === 1) {
                        $scope.account.companyId = value.data[0].CompanyId.toString();
                        $scope.getLocationByCompanyForDropdown();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }

            getAllCompanyForDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });