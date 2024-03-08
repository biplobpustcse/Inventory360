myApp
    .controller('replacementReceiveAnalysisReportController', ['$scope', '$q', 'applicationBasePath', 'userService',
        'locationCommonService', 'customerGroupCommonService', 'currencyCommonService', 'productGroupCommonService',
        'productSubGroupCommonService', 'productCategoryCommonService', 'productBrandCommonService', 'productModelCommonService',
        'productCommonService', 'supplierGroupCommonService', 'supplierCommonService', 'replacementReceiveAnalysisReportNameCommonService','replacementReceiveCommonService',
        function (
            $scope, $q, applicationBasePath, userService,
            locationCommonService, customerGroupCommonService, currencyCommonService, productGroupCommonService,
            productSubGroupCommonService, productCategoryCommonService, productBrandCommonService, productModelCommonService,
            productCommonService, supplierGroupCommonService, supplierCommonService, replacementReceiveAnalysisReportNameCommonService, replacementReceiveCommonService
        ) {
            $scope.selectedSupplierId = 0;
            $scope.selectedProductId = 0;

            $scope.getReplacementReceiveNumber = function () {
                var q = $scope.replacementReceiveNumber === undefined ? '' : $scope.replacementReceiveNumber;

                if (q.length >= 3) {
                    replacementReceiveCommonService.FetchReplacementReceiveNumberr(
                        q
                    ).then(function (value) {
                        $scope.replacementReceiveNumberJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedDeliveryId = "";
                    $scope.replacementReceiveNumberJsonData = null;
                }
            };

            $scope.fillreplacementReceiveNumberTextbox = function (obj) {
                $scope.selectedDeliveryId = obj.Value;
                $scope.replacementReceiveNumber = obj.Item;
                $scope.replacementReceiveNumberJsonData = null;
            };
            //getSupplierName
            $scope.getSupplierName = function () {
                var q = $scope.supplierName === undefined ? '' : $scope.supplierName;
                if (q.length >= 3) {
                    supplierCommonService.FetchAllSupplier(
                        q
                    ).then(function (value) {
                        $scope.supplierNameJsonData = value.data;
                    });
                } else {
                    $scope.selectedSupplierId = 0;
                    $scope.supplierNameJsonData = null;
                }
            };
            $scope.fillSupplierTextbox = function (obj) {
                $scope.selectedSupplierId = Number(obj.Value);
                $scope.supplierName = obj.Item;
                $scope.supplierNameJsonData = null;
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
                var url = applicationBasePath + "/Inventory360Reports/PrintReplacementReceiveAnalysis?reportName=" + $scope.reportName
                    + "&locationId=" + $scope.locationId
                    + "&dateFrom=" + ($scope.dateFrom == undefined ? '' : $scope.dateFrom)
                    + "&dateTo=" + ($scope.dateTo == undefined ? '' : $scope.dateTo)
                    + "&replacementReceiveId=" + ($scope.selectedReceiveId == undefined ? '' : $scope.selectedReceiveId)
                    + "&supplierGroupId=" + $scope.supplierGroupId
                    + "&supplierId=" + $scope.selectedSupplierId
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

            var GetSupplierGroupForDropdown = function () {
                supplierGroupCommonService.FetchSupplierGroup(
                    ''
                ).then(function (value) {
                    $scope.supplierGroupDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.supplierGroupId = this.Value;
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
                replacementReceiveAnalysisReportNameCommonService.FetchReplacementReceiveAnalysisReportName().then(function (value) {
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
            GetSupplierGroupForDropdown();
            GetCurrencyForDropdown();
            GetProductGroupForDropdown();
            GetBrandForDropdown();
            GetReportNameForDrowdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });