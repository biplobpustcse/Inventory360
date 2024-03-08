myApp
    .controller('transferOrderController', ['$scope', '$ngConfirm', '$filter', 'dataTableRows', 'applicationBasePath', 'userService',
        'employeeCommonService', 'productCommonService', 'stockTypeCommonService', 'locationCommonService',
        'transferRequisitionFinalizeWithDetailService', 'transferOrderService',
        function (
            $scope, $ngConfirm, $filter, dataTableRows, applicationBasePath, userService,
            employeeCommonService, productCommonService, stockTypeCommonService, locationCommonService,
            transferRequisitionFinalizeWithDetailService, transferOrderService
        ) {
            $scope.selectedEmployeeId = 0;
            $scope.selectedProductId = 0;
            $scope.isAdded = false;
            $scope.isSaved = false;
            $scope.isAgainstRequisition = false;
            $scope.requisitionFinalizeJsonData = [];
            $scope.addedProductLists = [];
            $scope.transferOrderDate = $filter('date')(new Date(), "M/d/yyyy");

            $scope.clickToReset = function () {
                $scope.selectedProductId = 0;
                $scope.productName = undefined;
                $scope.GivenQuantity = undefined;
                $scope.unitTypeDropDownJsonData = [];
                $scope.productDimensionDropDownJsonData = [];
            };

            $scope.getEmployeeName = function () {
                var q = $scope.requisitionBy === undefined ? '' : $scope.requisitionBy;

                if (q.length >= 3) {
                    employeeCommonService.FetchAllEmployee(
                        q
                    ).then(function (value) {
                        $scope.employeeJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedEmployeeId = 0;
                    $scope.employeeJsonData = null;
                }
            };

            $scope.fillEmployeeTextbox = function (obj) {
                $scope.selectedEmployeeId = Number(obj.Value);
                $scope.requisitionBy = obj.Item;
                $scope.employeeJsonData = null;
            };

            $scope.clickAgainstRequisition = function () {
                $scope.isAgainstRequisition = !$scope.isAgainstRequisition;
                $scope.requisitionFinalizeJsonData = [];
                if ($scope.isAgainstRequisition) {
                    LoadStockRequisitionNo();
                }
            };

            $scope.clickCheckAll = function (checkAllProduct, requisitionId, requisitionNo, detailLists) {
                $.each(detailLists, function () {
                    this.isSelected = checkAllProduct;
                    if (this.isSelected) {
                        addProductToAddedListsFromRequisition(requisitionId, requisitionNo, this);
                    } else {
                        deleteProductFromAddedListsByRequisition(requisitionId, this);
                    }
                });
            };

            $scope.clickCheckIndividual = function (checkIndividualProduct, requisitionId, requisitionNo, detailList) {
                detailList.isSelected = checkIndividualProduct;

                if (detailList.isSelected && parseFloat(detailList.GivenQuantity) > 0) {
                    addProductToAddedListsFromRequisition(requisitionId, requisitionNo, detailList);
                } else {
                    deleteProductFromAddedListsByRequisition(requisitionId, detailList);
                }
            };

            $scope.checkQuantity = function (requisitionId, item) {
                var remainingQty = item.Quantity - item.OrderQuantity;
                if (parseFloat(item.GivenQuantity) > remainingQty) {
                    item.GivenQuantity = remainingQty;
                }

                if (item.GivenQuantity === undefined || parseFloat(item.GivenQuantity) === 0) {
                    item.GivenQuantity = '0';
                    item.isSelected = false;
                    deleteProductFromAddedListsByRequisition(requisitionId, item);
                } else {
                    var identity = ProductIdentity(item.ProductId, item.ProductDimensionId, item.UnitTypeId);
                    $.each($scope.addedProductLists, function (index, list) {
                        if (list.RequisitionFinalizeId === requisitionId && list.identity === identity) {
                            list.Quantity = parseFloat(item.GivenQuantity);

                            return false;
                        }
                    });
                }

                sortingAddedProductLists();
            };

            $scope.getProductName = function () {
                var q = $scope.productName === undefined ? '' : $scope.productName;

                if (q.length >= 2) {
                    productCommonService.FetchProductName(
                        q
                    ).then(function (value) {
                        $scope.productNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedProductId = 0;
                    $scope.productNameJsonData = null;
                }
            };

            $scope.fillProductTextbox = function (obj) {
                $scope.selectedProductId = Number(obj.Value);
                $scope.productName = obj.Item;
                $scope.productNameJsonData = null;
                GetProductWiseDimension();
                GetProductWiseUnitType();
            };

            $scope.getDimensionName = function () {
                $.each($scope.productDimensionDropDownJsonData, function () {
                    if (this.Value === $scope.dimensionId) {
                        $scope.dimensionName = this.Item;
                    }
                });
            };

            $scope.getUnitTypeName = function () {
                $.each($scope.unitTypeDropDownJsonData, function () {
                    if (this.Value === $scope.unitTypeId) {
                        $scope.unitTypeName = this.Item;
                    }
                });
            };

            $scope.clickToAdd = function () {
                $scope.addedProductLists.push({
                    "identity": ProductIdentity($scope.selectedProductId, ($scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0'), $scope.unitTypeId.toString()),
                    "noOfRows": 0,
                    "totalQuantity": 0,
                    "IsFirstRow": true,
                    "ProductId": $scope.selectedProductId.toString(),
                    "ProductDimensionId": $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0',
                    "Code": $scope.productCode,
                    "Name": $scope.productName,
                    "Dimension": $scope.dimensionName,
                    "UnitTypeId": $scope.unitTypeId.toString(),
                    "UnitTypeName": $scope.unitTypeName,
                    "Quantity": parseFloat($scope.GivenQuantity),
                    "RequisitionFinalizeId": null,
                    "RequisitionNo": null
                });

                $scope.clickToReset();
                sortingAddedProductLists();
            };

            $scope.clickRemoveItem = function (identity, requisitionId) {
                $.each($scope.addedProductLists, function (index) {
                    if (this.identity === identity && this.RequisitionFinalizeId === requisitionId) {
                        $scope.addedProductLists.splice(index, 1);
                        return false;
                    }
                });

                sortingAddedProductLists();
            };

            $scope.clickToReset = function () {
                $scope.selectedProductId = 0;
                $scope.productName = undefined;
                $scope.GivenQuantity = undefined;
                $scope.unitTypeDropDownJsonData = [];
                $scope.productDimensionDropDownJsonData = [];
            };

            $scope.clickToSave = function () {
                if ($scope.addedProductLists.length === 0) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Have not added any product.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }

                if ($scope.transferOrderNo != undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'This requisition finalize already saved.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }

                $scope.isSaved = true;
                transferOrderService.SaveTransferOrder(
                    $scope.transferOrderDate, $scope.selectedEmployeeId, $scope.remarks, $scope.addedProductLists,
                    $scope.requisitionTo, $scope.requisitionFrom, $scope.stockTypeTo, $scope.stockTypeFrom
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.transferOrderNo = value.data.Message;
                        // alert
                        $ngConfirm({
                            title: 'Success',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: 'Save successful.',
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                    } else {
                        // alert
                        $ngConfirm({
                            title: 'Failed',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: value.data,
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                        $scope.isSaved = false;
                    }
                });
            };
            $scope.PrintReport = function () {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndTransferOrder?no=" + $scope.transferOrderNo + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            var ProductIdentity = function (productId, productDimensionId, unitTypeId) {
                return productId.toString() + (productDimensionId === null ? '0' : productDimensionId.toString()) + unitTypeId.toString();
            };

            var GetProductWiseDimension = function () {
                productCommonService.FetchProductWiseDimension(
                    '', $scope.selectedProductId
                ).then(function (value) {
                    $scope.productDimensionDropDownJsonData = value.data;
                    if (value.data.length > 0) {
                        $scope.dimensionId = value.data[0].Value;
                        $scope.getDimensionName();
                    } else {
                        $scope.dimensionName = '';
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }

            var GetProductWiseUnitType = function () {
                productCommonService.FetchProductWiseUnitType(
                    '', $scope.selectedProductId
                ).then(function (value) {
                    $scope.unitTypeDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.unitTypeId = this.Value;
                            $scope.getUnitTypeName();
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            var LoadStockRequisitionNo = function () {
                var q = $scope.searchRequisitionNo === undefined ? '' : $scope.searchRequisitionNo;

                transferRequisitionFinalizeWithDetailService.FetchRequisitionFinalizeWithDetaill(
                    q
                ).then(function (value) {
                    $scope.requisitionFinalizeJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }

            var addProductToAddedListsFromRequisition = function (requisitionId, requisitionNo, item) {
                $scope.addedProductLists.push({
                    "identity": ProductIdentity(item.ProductId, item.ProductDimensionId, item.UnitTypeId),
                    "noOfRows": 0,
                    "totalQuantity": 0,
                    "IsFirstRow": true,
                    "ProductId": item.ProductId.toString(),
                    "ProductDimensionId": item.ProductDimensionId == null ? '0' : item.ProductDimensionId.toString(),
                    "Code": item.ProductCode,
                    "Name": '[' + item.ProductCode + '] ' + item.ProductName,
                    "Dimension": item.ProductDimension == null ? '' : item.ProductDimension,
                    "UnitTypeId": item.UnitTypeId.toString(),
                    "UnitTypeName": item.UnitType,
                    "Quantity": parseFloat(item.GivenQuantity),
                    "RequisitionFinalizeId": requisitionId,
                    "RequisitionNo": requisitionNo
                });

                sortingAddedProductLists();
            }

            var deleteProductFromAddedListsByRequisition = function (requisitionId, item) {
                $.each($scope.addedProductLists, function (index) {
                    var identity = ProductIdentity(item.ProductId, item.ProductDimensionId, item.UnitTypeId);
                    if (this.RequisitionFinalizeId === requisitionId && this.identity === identity) {
                        $scope.addedProductLists.splice(index, 1);
                        return false;
                    }
                });

                sortingAddedProductLists();
            }

            var sortingAddedProductLists = function () {
                $scope.addedProductLists.sort(function (a, b) {
                    return a.identity - b.identity;
                });

                var result = [];
                $scope.addedProductLists.reduce(function (res, value) {
                    var identity = ProductIdentity(value.ProductId, value.ProductDimensionId, value.UnitTypeId);
                    if (!res[identity]) {
                        res[identity] = {
                            noOfRows: 0,
                            totalQuantity: 0,
                            identity: identity
                        };
                        result.push(res[identity]);
                    }

                    res[identity].noOfRows += 1;
                    res[identity].totalQuantity += value.Quantity;

                    return res;
                }, {});

                // set no of rows and total quantity to main array
                $.each(result, function (index1, item1) {
                    var isFirstRow = true;
                    $.each($scope.addedProductLists, function (index, item) {
                        if (item1.identity === item.identity) {
                            item.noOfRows = item1.noOfRows;
                            item.totalQuantity = item1.totalQuantity;
                            item.IsFirstRow = isFirstRow;

                            isFirstRow = false;
                        }
                    });
                });

                $scope.isAdded = $scope.addedProductLists.length > 0;
            }

            //load Product Stock Type Drop Down
            var GetProductStockTypeDropdown = function () {
                stockTypeCommonService.FetchProductStockType().then(function (value) {
                    $scope.productStockTypeDropdownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.stockTypeTo = this.Value;
                            $scope.stockTypeFrom = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }

            var GetLocationListDropdown = function () {
                locationCommonService.FetchLocationByCompany().then(function (value) {
                    $scope.locationListDropdownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.requisitionTo = this.Value;
                            $scope.requisitionFrom = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }

            GetProductStockTypeDropdown();
            GetLocationListDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });