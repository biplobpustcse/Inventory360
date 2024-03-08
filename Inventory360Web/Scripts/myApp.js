var myApp = angular.module('myApp', ['cp.ngConfirm', 'angular-loading-bar', 'textAngular']);

// global variables
myApp.constant('serviceBasePath', 'http://localhost:57488');
myApp.constant('applicationBasePath', 'http://localhost:61637');
myApp.constant('dataTableRows', '10');

// to get logged userinfo and display
myApp
    .controller('loggedInfoController', ['$scope', 'userService', 'userInfoService', 'applicationBasePath',
        function ($scope, userService, userInfoService, applicationBasePath) {
            $scope.CompanyName = '';
            $scope.LocationName = '';
            $scope.LocationId = '';
            $scope.UserName = '';
            $scope.DefaultCurrency = '';

            var ShowLoggedUserInfo = function () {
                var currentUser = userService.GetCurrentUser();
                if (currentUser === undefined) {
                    window.location.replace(applicationBasePath);
                    return;
                }

                var loggedUserInfo = userService.GetLoggedUserInfo();
                if (loggedUserInfo === null || loggedUserInfo === undefined) {
                    userInfoService.GetLoggedUserData().then(function (data) {
                        userService.SetLoggedUserInfo(data);
                        $scope.CompanyName = data.CompanyName;
                        $scope.LocationName = data.LocationName;
                        $scope.LocationId = data.LocationId;
                        $scope.UserName = data.UserName;
                        $scope.DefaultCurrency = data.DefaultCurrency;
                        }, function (error) {
                        });
                }
                else {
                    $scope.CompanyName = loggedUserInfo.CompanyName;
                    $scope.LocationName = loggedUserInfo.LocationName;
                    $scope.UserName = loggedUserInfo.UserName;
                    $scope.LocationId = loggedUserInfo.LocationId;
                    $scope.DefaultCurrency = loggedUserInfo.DefaultCurrency;
                }
            }

            ShowLoggedUserInfo();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });

// http interceptor
myApp.config(['$httpProvider', function ($httpProvider) {
    var interceptor = function (userService, $q) {
        return {
            request: function (config) {
                var currentUser = userService.GetCurrentUser();
                if (currentUser !== null && currentUser !== undefined) {
                    config.headers['Authorization'] = 'Bearer ' + currentUser.access_token;
                }
                return config;
            },
            responseError: function (rejection) {
                if (rejection.status === 401) {
                    return $q.reject(rejection);
                }

                if (rejection.status === 403) {
                    rejection.data = 'you have no permission.';
                    return $q.reject(rejection);
                }

                return $q.reject(rejection);
            }
        }
    }

    var params = ['userService', '$q'];
    interceptor.$inject = params;

    $httpProvider.interceptors.push(interceptor);
}]);