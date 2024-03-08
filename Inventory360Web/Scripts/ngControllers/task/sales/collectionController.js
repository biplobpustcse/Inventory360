myApp
    .controller('collectionController', ['$scope', '$filter', '$ngConfirm', 'applicationBasePath', 'userService', 'currencyCommonService', 'customerCommonService', 'securityUserCommonService', 'collectionAgainstCommonService', 'paymentModeCommonService', 'bankCommonService', 'collectionService',
        function (
            $scope, $filter, $ngConfirm, applicationBasePath, userService, currencyCommonService,
            customerCommonService, securityUserCommonService, collectionAgainstCommonService, paymentModeCommonService,
            bankCommonService, collectionService
        ) {
            $scope.needDetail = false;
            $scope.selectedCustomerOrBuyerId = 0;
            $scope.selectedSalesPersonId = 0;
            $scope.selectedCurrency = $scope.DefaultCurrency;
            $scope.collectionDate = $filter('date')(new Date(), "M/d/yyyy");
            $scope.addedCollectionDetail = [];
            $scope.addedCollectionMapping = [];

            $scope.loadExchangeRate = function () {
                $scope.selectedCurrency = $scope.currencyId;
                GetCurrencyRate();
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
                    $scope.selectedCustomerOrBuyerId = 0;
                    $scope.customerNameJsonData = null;
                }
            };

            $scope.fillCustomerTextbox = function (obj) {
                $scope.customerName = '';
                $scope.selectedCustomerOrBuyerId = Number(obj.Value);
                $scope.customerName = obj.Item;
                $scope.customerNameJsonData = null;
                getCustomerShortInfo();
                // TODO : Must load Customer MIS like - Ledger Balance, Credit Limit, Current Due, Collected Amount
                // load collection mapping data for collection mapping grid
                $scope.showCollectionMapping();
            };

            // must load collection mapping data for collection mapping grid
            $scope.showCollectionMapping = function () {
                collectionService.GetCollectionMappingData(
                    $scope.collectionAgainstId,
                    $scope.currencyId,
                    $scope.selectedCustomerOrBuyerId,
                    $scope.companyId
                ).then(function (value) {
                    $scope.collectionMappingDetail = value.data;
                    $scope.autoMappingByCollection();
                    $.each($scope.collectionAgainstDropDownJsonData, function () {
                        if (this.Value === $scope.collectionAgainstId) {
                            $scope.selectedCollectionAgainst = this.Item;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.autoMappingByCollection = function () {
                var collectedAmount = parseFloat($scope.collectedAmount === undefined ? '0' : $scope.collectedAmount);
                $.each($scope.collectionMappingDetail, function () {
                    var remainingAmount = (this.Amount - this.CollectedAmount);
                    var setAmount = collectedAmount > remainingAmount ? remainingAmount : collectedAmount;
                    collectedAmount -= setAmount;
                    this.GivenAmount = setAmount;
                    this.isSelected = setAmount > 0;
                });
            };

            $scope.checkedAll = function () {
                $.each($scope.collectionMappingDetail, function () {
                    this.isSelected = $scope.checkAll;
                });
            };

            $scope.clickMultiPaymentMode = function () {
                $scope.isMultiPaymentMode = !$scope.isMultiPaymentMode;
            };

            $scope.checkAmount = function (item) {
                var remainingAmount = (item.Amount - item.CollectedAmount);
                if (parseFloat(item.GivenAmount) > remainingAmount) {
                    item.GivenAmount = remainingAmount;
                }

                item.isSelected = !(item.GivenAmount === undefined || parseFloat(item.GivenAmount) === 0);
            };

            $scope.showPaymentDetail = function () {
                $.each($scope.paymentModeDropDownJsonData, function () {
                    if (this.Value === $scope.paymentModeId) {
                        $scope.selectedPaymentMode = this.Item;
                        $scope.needDetail = this.NeedDetail;
                    }
                });
            };

            $scope.getBankName = function () {
                $.each($scope.bankDropDownJsonData, function () {
                    if (this.Value === $scope.bankId) {
                        $scope.selectedBankName = this.Item;
                    }
                });
            };

            $scope.addCollectionInfo = function () {
                var sameChequeNoFound = false;
                $.each($scope.addedCollectionDetail, function () {
                    if (this.ChequeNo === $scope.chequeNo) {
                        sameChequeNoFound = true;
                    }
                });

                if (sameChequeNoFound) {
                    return;
                }

                if ($scope.needDetail) {
                    $scope.addedCollectionDetail.push({
                        "PaymentModeId": $scope.paymentModeId,
                        "PaymentMode": $scope.selectedPaymentMode,
                        "Amount": $scope.collectedAmount,
                        "BankId": $scope.bankId,
                        "BankName": $scope.selectedBankName,
                        "ChequeNo": $scope.chequeNo,
                        "ChequeDate": $scope.chequeDate
                    });
                }
                else {
                    $scope.addedCollectionDetail.push({
                        "PaymentModeId": $scope.paymentModeId,
                        "PaymentMode": $scope.selectedPaymentMode,
                        "Amount": $scope.collectedAmount,
                        "BankId": null,
                        "BankName": null,
                        "ChequeNo": null,
                        "ChequeDate": null
                    });
                }
            };

            $scope.clickToSave = function () {
                if ($scope.collectionNo != undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'This collection already saved.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }

                // collection detail
                $scope.addCollectionInfo();
                // collection mapping
                $.each($scope.collectionMappingDetail, function () {
                    if (this.isSelected) {
                        $scope.addedCollectionMapping.push({
                            "SalesOrderId": $scope.collectionAgainstId === "SO" ? this.Id : null,
                            "InvoiceId": $scope.collectionAgainstId === "Inv" ? this.Id : null,
                            "Amount": this.GivenAmount
                        });
                    }
                });

                $scope.isSaved = true;
                collectionService.SaveSalesCollection(
                    $scope.collectionDate, $scope.currencyId, $scope.exchangeRate,
                    $scope.collectedAmount, $scope.selectedCustomerOrBuyerId, $scope.selectedSalesPersonId,
                    $scope.collectedById, $scope.moneyReceiptNo, $scope.remarks,
                    $scope.addedCollectionDetail, $scope.addedCollectionMapping
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {                        
                        $scope.collectionNo = value.data.Message;
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
                            content: value.data.Message,
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
                var url = applicationBasePath + "/Inventory360Reports/PrintIndCollection?no=" + $scope.collectionNo + "&user=" + $scope.UserName + "&currency=" + $scope.currencyId + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                    $scope.currencyIdForGrid = $scope.DefaultCurrency;
                    GetCurrencyRate();
                }, function (reason) {
                    console.log(reason);
                });
            }

            var GetCurrencyRate = function () {
                currencyCommonService.FetchCurrencyRate(
                    $scope.currencyId
                ).then(function (value) {
                    $scope.exchangeRate = value.data.ExchangeRate;
                    $scope.baseCurrency = value.data.BaseCurrency;
                    $scope.isBaseCurrencySelected = ($scope.baseCurrency === $scope.currencyId);
                }, function (reason) {
                    console.log(reason);
                });
            }

            var getCustomerShortInfo = function () {
                customerCommonService.FetchCustomerShortInfo(
                    $scope.selectedCustomerOrBuyerId
                ).then(function (value) {
                    $scope.customerGroup = value.data.GroupName;
                    $scope.customerAddress = value.data.Address;
                    $scope.salesPersonName = value.data.SalesPersonName;
                    $scope.selectedSalesPersonId = value.data.SalesPersonId;
                }, function (reason) {
                    console.log(reason);
                });
            }

            var GetSecurityUser = function () {
                securityUserCommonService.FetchLocationWiseSecurityUserAll(
                    ''
                ).then(
                    function (value) {
                        $scope.collectedByDropDownJsonData = value.data;
                        $.each(value.data, function () {
                            if (this.Item.toLowerCase().search($scope.UserName.toLowerCase()) >= 0) {
                                $scope.collectedById = this.Value;
                            }
                        });
                    }, function (reason) {
                        console.log(reason);
                    }
                );
            }

            var GetCollectionAgainstForDropdown = function () {
                collectionAgainstCommonService.FetchCollectionAgainst().then(function (value) {
                    $scope.collectionAgainstDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.collectionAgainstId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }

            var GetPaymentMode = function () {
                paymentModeCommonService.FetchPaymentMode().then(
                    function (value) {
                        $scope.paymentModeDropDownJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    }
                );
            }

            var GetBankForDropdown = function () {
                bankCommonService.FetchBank(
                    ''
                ).then(function (value) {
                    $scope.bankDropDownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }

            GetCurrencyForDropdown();
            GetSecurityUser();
            GetCollectionAgainstForDropdown();
            GetPaymentMode();
            GetBankForDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });