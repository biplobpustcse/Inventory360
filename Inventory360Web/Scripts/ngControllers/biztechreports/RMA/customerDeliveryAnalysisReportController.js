myApp
    .controller('customerDeliveryAnalysisReportController', ['$scope', '$q', 'applicationBasePath', 'userService',
        'locationCommonService', 'customerGroupCommonService', 'currencyCommonService', 'productGroupCommonService',
        'productSubGroupCommonService', 'productCategoryCommonService', 'productBrandCommonService', 'productModelCommonService',
        'productCommonService', 'customerCommonService', 'customerDeliveryCommonService', 'customerDeliveryAnalysisReportNameCommonService',
        function (
            $scope, $q, applicationBasePath, userService,
            locationCommonService, customerGroupCommonService, currencyCommonService, productGroupCommonService,
            productSubGroupCommonService, productCategoryCommonService, productBrandCommonService, productModelCommonService,
            productCommonService, customerCommonService, customerDeliveryCommonService, customerDeliveryAnalysisReportNameCommonService
        ) {
            $scope.selectedDeliveryId = "";
            $scope.selectedCustomerId = 0;
            $scope.selectedProductId = 0;

            $scope.getDeliveryNumber = function () {
                var q = $scope.deliveryNumber === undefined ? '' : $scope.deliveryNumber;

                if (q.length >= 3) {
                    customerDeliveryCommonService.FetchDeliveryNumber(
                        q
                    ).then(function (value) {
                        $scope.deliveryNumberJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedDeliveryId = "";
                    $scope.deliveryNumberJsonData = null;
                }
            };

            $scope.fillDeliveryNumberTextbox = function (obj) {
                $scope.selectedDeliveryId = obj.Value;
                $scope.deliveryNumber = obj.Item;
                $scope.deliveryNumberJsonData = null;
            };

            $scope.getCustomerName = function () {
                var q = $scope.customerName === undefined ? '' : $scope.customerName;

                if (q.length >= 3) {
                    customerCommonService.FetchAllCustomer(
                        q
                    ).then(function (value) {
                        $scope.customerNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedCustomerId = 0;
                    $scope.customerNameJsonData = null;
                }
            };

            $scope.fillCustomerTextbox = function (obj) {
                $scope.selectedCustomerId = Number(obj.Value);
                $scope.customerName = obj.Item;
                $scope.customerNameJsonData = null;
            };

            // Load subgroup and category by selecte group
            $scope.getSubGroupAndCategoryForDropdown = function () {
                $scope.productSubGroupDropDownJsonData = null;
                $scope.productCategoryDropDownJsonData = null;
                $scope.modelDropDownJsonData = null;
                if (Number($scope.productGroupId) > 0) {
                    SequenceCall({ GetProductSubGroupForDropdown, GetProductCategoryForDropdown, GetProductModelForDropdown });
                }
            };

            $scope.getProductModelForDropdown = function () {
                GetProductModelForDropdown();
            };

            $scope.getProductName = function () {
                var q = $scope.productName === undefined ? '' : $scope.productName;

                if (q.length >= 2) {
                    productCommonService.FetchProductName1(
                        q, $scope.productGroupId, ($scope.productSubGroupId == undefined ? '0' : $scope.productSubGroupId), ($scope.productCategoryId == undefined ? '0' : $scope.productCategoryId),
                        $scope.brandId, ($scope.modelId == undefined ? '' : $scope.modelId)
                    ).then(function (value) {
                        $scope.productNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.productNameJsonData = null;
                    $scope.selectedProductId = 0;
                }
            };

            $scope.fillProductTextbox = function (obj) {
                $scope.selectedProductId = Number(obj.Value);
                $scope.productName = obj.Item;
                $scope.productNameJsonData = null;
            };

            $scope.PrintReport = function () {
                var url = applicationBasePath + "/Inventory360Reports/PrintCustomerDeliveryAnalysis?reportName=" + $scope.reportName
                    + "&locationId=" + $scope.locationId
                    + "&dateFrom=" + ($scope.dateFrom == undefined ? '' : $scope.dateFrom)
                    + "&dateTo=" + ($scope.dateTo == undefined ? '' : $scope.dateTo)
                    + "&customerDeliveryId=" + ($scope.selectedDeliveryId == undefined ? '' : $scope.selectedDeliveryId)
                    + "&customerGroupId=" + $scope.customerGroupId
                    + "&customerId=" + $scope.selectedCustomerId
                    + "&groupId=" + $scope.productGroupId
                    + "&subGroupId=" + $scope.productSubGroupId
                    + "&categoryId=" + $scope.productCategoryId
                    + "&brandId=" + $scope.brandId
                    + "&model=" + ($scope.modelId == undefined ? '' : $scope.modelId)
                    + "&productId=" + $scope.selectedProductId
                    + "&currencyId=" + $scope.currencyId
                    + "&user=" + $scope.UserName
                    + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            function IsPromiseLike(obj) {
                return obj && angular.isFunction(obj.then);
            };

            function SequenceCall(tasks) {
                //Fake a "previous task" for our initial iteration
                var prevPromise;
                var error = new Error();
                angular.forEach(tasks, function (task, key) {
                    var success = task.success || task;
                    var fail = task.fail;
                    var notify = task.notify;
                    var nextPromise;

                    //First task
                    if (!prevPromise) {
                        nextPromise = success();
                        if (!IsPromiseLike(nextPromise)) {
                            error.message = "Task " + key + " did not return a promise.";
                            throw error;
                        }
                    } else {
                        //Wait until the previous promise has resolved or rejected to execute the next task
                        nextPromise = prevPromise.then(
            /*success*/function (data) {
                                if (!success) { return data; }
                                var ret = success(data);
                                if (!IsPromiseLike(ret)) {
                                    error.message = "Task " + key + " did not return a promise.";
                                    throw error;
                                }
                                return ret;
                            },
                            notify);
                    }
                    prevPromise = nextPromise;
                });

                return prevPromise;
            };

            var GetProductSubGroupForDropdown = function () {
                var d = $q.defer();

                productSubGroupCommonService.FetchProductSubGroup(
                    '',
                    Number($scope.productGroupId)
                ).then(function (value) {
                    $scope.productSubGroupDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.productSubGroupId = this.Value;
                        }
                    });

                    d.resolve('GetProductSubGroupForDropdown');
                }, function (reason) {
                    console.log(reason);
                });

                return d.promise;
            };

            var GetProductCategoryForDropdown = function () {
                var d = $q.defer();

                productCategoryCommonService.FetchProductCategory(
                    '',
                    Number($scope.productGroupId)
                ).then(function (value) {
                    $scope.productCategoryDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.productCategoryId = this.Value;
                        }
                    });

                    d.resolve('GetProductCategoryForDropdown');
                }, function (reason) {
                    console.log(reason);
                });

                return d.promise;
            };

            var GetProductModelForDropdown = function () {
                var d = $q.defer();

                productModelCommonService.FetchProductModel(
                    $scope.productGroupId, ($scope.productSubGroupDropDownJsonData.length == 0 ? '0' : $scope.productSubGroupId), ($scope.productCategoryDropDownJsonData.length == 0 ? '0' : $scope.productCategoryId), $scope.brandId
                ).then(function (value) {
                    $scope.modelDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.modelId = this.Value;
                        }
                    });

                    d.resolve('GetProductModelForDropdown');
                }, function (reason) {
                    console.log(reason);
                });

                return d.promise;
            }

            // load location which are login location
            var GetLocationForDropdown = function () {
                locationCommonService.FetchLocationByCompany().then(function (value) {
                    $scope.locationDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.locationId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            var GetCustomerGroupForDropdown = function () {
                customerGroupCommonService.FetchCustomerGroup(
                    ''
                ).then(function (value) {
                    $scope.customerGroupDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.customerGroupId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                }, function (reason) {
                    console.log(reason);
                });
            };

            // load product group
            var GetProductGroupForDropdown = function () {
                productGroupCommonService.FetchProductGroup(
                    ''
                ).then(function (value) {
                    $scope.productGroupDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.productGroupId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            var GetBrandForDropdown = function () {
                productBrandCommonService.FetchProductBrand(
                    ''
                ).then(function (value) {
                    $scope.brandDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.brandId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            var GetReportNameForDrowdown = function () {
                customerDeliveryAnalysisReportNameCommonService.FetchcustomerDeliveryAnalysisReportName().then(function (value) {
                    $scope.reportNameDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.reportName = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            GetLocationForDropdown();
            GetCustomerGroupForDropdown();
            GetCurrencyForDropdown();
            GetProductGroupForDropdown();
            GetBrandForDropdown();
            GetReportNameForDrowdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });