myApp
    .controller('transferChallanController', ['$scope', '$ngConfirm','$filter', 'dataTableRows', 'applicationBasePath', 'userService',
        'employeeCommonService', 'productCommonService', 'stockTypeCommonService', 'locationCommonService',
        'transferOrderWithDetailService', 'transferOrderService',
        function (
            $scope, $ngConfirm, $filter, dataTableRows, applicationBasePath, userService,
            employeeCommonService, productCommonService, stockTypeCommonService, locationCommonService,
            transferOrderWithDetailService, transferOrderService,
        ) {
            $scope.selectedEmployeeId = 0;
            $scope.selectedProductId = 0;
            $scope.selectedOrderId = 0;
            $scope.isAdded = false;
            $scope.isSaved = false;
            $scope.transferorderJsonData = [];
            $scope.warehouseJsonData = [];
            $scope.transferOrderDate = $filter('date')(new Date(), "M/d/yyyy");
            $scope.ProductSelectedSerailList = [];
            $scope.addedProductLists = [];
            $scope.addedWarehouseListsWithProduct = [];

            $scope.clickCheckIndividualSerialNo = function (isSelected, SerailNo, StockDetailId,productId) {
                if (isSelected == true) {
                    $scope.ProductSelectedSerailList.push({
                        'SerailNo': SerailNo,
                        'StockDetailId': StockDetailId,
                        'ProductId': productid
                    });
                }
                else {
                    $.each($scope.ProductSelectedSerailList, function (index) {
                        if (this.SerailNo == SerailNo && this.StockDetailId == StockDetailId) {
                            $scope.ProductSelectedSerailList.splice(index, 1);
                            //return false;
                        }
                    });
                }
            };

            $scope.clickCheckIndividual = function (isSelected, isSerialAvailable, orderId, requisitionNo, modalid, productid) {

                $scope.selectedProductId = productid;
                $scope.selectedOrderId = orderId;

                transferOrderService.FetchProductDetailInfo(productid, orderId)
                    .then(function (value) {
                        $scope.productInfoJsonData = value.data;
                        
                    }, function (reason) {
                        console.log(reason);
                    });


                if (modalid == 2) {//Warehouse 
                    transferOrderService.FetchProductWarehouseByLocation(productid, orderId)
                        .then(function (value) {
                            if (isSelected && $scope.ProductSelectedSerailList.length<1) {
                                $scope.warehouseJsonData = value.data;
                            }
                            //$.each(value.data, function () {

                            //});
                        }, function (reason) {
                            console.log(reason);
                        });
                }
                else {//Serial Product
                    transferOrderService.FetchProductSerialNos(productid, orderId)
                        .then(function (value) {
                            if (isSelected) {

                                $scope.allserialnoByProductJsonData = value.data;

                                $.each($scope.allserialnoByProductJsonData, function (index1, list1) {
                                    $.each($scope.ProductSelectedSerailList, function (index2, list2) {
                                        if (list1.SerailNo === list2.SerailNo && list1.StockDetailId === list2.StockDetailId) {
                                            $scope.allserialnoByProductJsonData[index1].IsSelected = true;
                                        }
                                    });
                                });
                                //var flag = false;
                                $.each($scope.transferorderJsonData, function (orderIndex, item) {
                                    $.each(item.DetailLists, function (detailIndex, dtlItem) {
                                        if (item.OrderId === orderId && dtlItem.ProductId === productid) {
                                            item.DetailLists[detailIndex].IsSelected = true;
                                            //flag = true;
                                            //break;
                                        }
                                    });
                                    //if (flag === true) { break; }
                                });

                            }
                            else {
                                $scope.allserialnoByProductJsonData = value.data;
                                $.each($scope.allserialnoByProductJsonData, function (allSerialIndex, item) {
                                    $.each($scope.ProductSelectedSerailList, function (addedSerailIndex, addedtem) {
                                        if (item.ProductId === addedtem.ProductId && item.SerialNo === addedtem.SerialNo) {
                                            $scope.ProductSelectedSerailList.splice(addedSerailIndex,1)
                                        }
                                    });
                                });
                                
                            }
                            transferOrderService.FetchProductWarehouseByLocationForSerialProduct(productid, orderId)
                                .then(function (value) {
                                    if (!isSelected) {
                                        $scope.warehouseJsonDataForSerialProduct = value.data;
                                    }
                                }, function (reason) {
                                    console.log(reason);
                                });

                        }, function (reason) {
                            console.log(reason);
                        });
                }
            };

            $scope.AddWarehouseBasedProductQuantity = function (isSelected,warehouseid, productid, UnitTypeId, DimentionId, qty) {
                if (isSelected && qty == 0) {
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Have not Entered any Quantity.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });

                    $.each($scope.warehouseJsonData, function (index, warehouse) {
                        if (warehouse.WareHouseId == warehouseid && warehouse.IsSelected) {
                            warehouse.IsSelected = false;
                        }
                    });

                    return;
                }
                if (isSelected) {
                    $scope.addedWarehouseListsWithProduct.push({
                        'WarehouseId': warehouseid,
                        'ProductId': productid,
                        'UnitTypeId': UnitTypeId,
                        'DimentionId': DimentionId,
                        'Quantity': qty
                    });
                }
            };
            $scope.AddProductSerial = function () {
                alert("Under Construction");
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
           
            $scope.PrintReport = function () {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndTransferOrder?no=" + $scope.transferOrderNo + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            var ProductIdentity = function (productId, productDimensionId, unitTypeId) {
                return productId.toString() + (productDimensionId === null ? '0' : productDimensionId.toString()) + unitTypeId.toString();
            };            

            $scope.LoadProductSerial = function () {
                var warehouseId = $scope.warehouseId;
                var productid = $scope.selectedProductId;
                var orderid = $scope.selectedOrderId;
                transferOrderService.FetchWarehouseBasedSerialNo(warehouseId, productid, orderid)
                    .then(function (value) {
                        $scope.warehouseBasedSerialNoJsonData = value.data;
                        //$.each(value.data, function () {

                        //});
                    }, function (reason) {
                        console.log(reason);
                    });
            };
            $scope.LoadTransferOrder = function () {
                var requisitionTo = $scope.requisitionTo;
                var stockTypeFrom = $scope.stockTypeFrom;
                var stockTypeTo = $scope.stockTypeTo;
                var fromDate = "";
                if ($scope.transferOrderDateFrom != "")
                    fromDate = $scope.transferOrderDateFrom;

                var toDate = "";
                if ($scope.transferOrderDateTo != "")
                    toDate = $scope.transferOrderDateTo;

                transferOrderService.FetchTransferOrderSearch(requisitionTo, fromDate, toDate, stockTypeFrom, stockTypeTo)
                    .then(function (value) {
                        debugger;
                    $scope.transferorderJsonData = value.data.Data;
                        //$.each(value.data, function () {
                        //alert("");
                    //});
                }, function (reason) {
                    console.log(reason);
                });

                //alert(requisitionTo + ',' + stockTypeFrom + ',' + stockTypeTo + "," + fromDate + "," + toDate);
            }

            //var LoadTransferOrder = function () {
            //    stockTypeCommonService.FetchProductStockType().then(function (value) {
            //        $scope.transferOrderDropdownJsonData = value.data;
            //        $.each(value.data, function () {
            //            if (this.IsSelected) {
            //                //$scope.transferOrderNo = this.Value;
            //            }
            //        });
            //    }, function (reason) {
            //        console.log(reason);
            //    });
            //}

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
                locationCommonService.FetchLocationByCompanyExceptOwnLocation().then(function (value) {
                    $scope.locationListDropdownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.requisitionTo = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }
            //LoadTransferOrder();
            GetProductStockTypeDropdown();
            GetLocationListDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });