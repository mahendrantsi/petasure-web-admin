const token = document.getElementsByName("__RequestVerificationToken")[0].value;
async function initCheckout(clientKey, type, payAmount, payIncurrency, model, submitDataUrl, transactionRef, typeOfTransaction,returnArea) {
  
    try {
        const amt = payAmount * 100;
        const paymentMethodsResponse = await callServer("/Adyen/getPaymentMethods", {
            merchantAccount: "",
        });
        const configuration = {
            paymentMethodsResponse: filterUnimplemented(paymentMethodsResponse),
            clientKey,
            locale: "en_US",
            environment: "test",
            showPayButton: true,
            paymentMethodsConfiguration: {
                ideal: {
                    showImage: true,
                },
                card: {
                    hasHolderName: true,
                    holderNameRequired: true,
                    enableStoreDetails: true,
                    hideCVC: false,
                    name: "Credit or debit card",
                    amount: {
                        value: amt,
                        currency: payIncurrency,
                    },
                },
            },
            onSubmit: (state, component) => {
               
                if (state.isValid) {
                    state.data.amount = {
                        currency: payIncurrency,
                        value: amt,
                    };
                    const obj =
                    {
                        paymentReq: state.data,
                        transactionNumber: transactionRef,
                        typeOfTransaction: typeOfTransaction,
                    };
                    handleSubmission(obj, component, "/Adyen/initiatePayment", model, submitDataUrl,returnArea);
                }
            },
            onAdditionalDetails: (state, component) => {
                handleSubmission(state.data, component, "/Adyen/submitAdditionalDetails", model, submitDataUrl,returnArea);
            },
        };

        const checkout = new AdyenCheckout(configuration);
        checkout.create(type).mount(document.getElementById(type));
    } catch (error) {
        console.error(error);
        alert("Error occurred. Look at console for details");
    }
}
function filterUnimplemented(pm) {
    pm.paymentMethods = pm.paymentMethods.filter((it) =>
        [
            "ach",
            "scheme",
            "dotpay",
            "giropay",
            "ideal",
            "directEbanking",
            "klarna_paynow",
            "klarna",
            "klarna_account",
        ].includes(it.type)
    );
    return pm;
}

// Event handlers called when the shopper selects the pay button,
// or when additional information is required to complete the payment
async function handleSubmission(state, component, url, model, submitDataUrl, returnArea) {
    try {
        const res = await callServer(url, state);
        handleServerResponse(res, component, model, submitDataUrl,returnArea);
    } catch (error) {
        console.error(error);
        alert("Error occurred. Look at console for details");
    }
}

// Calls your server endpoints
async function callServer(url, data) {
    const res = await fetch(url, {
        method: "POST",
        body: data ? JSON.stringify(data) : "",
        headers: {
            "RequestVerificationToken": token,
            "Content-Type": "application/json",
        },
    });
    return await res.json();
}

// Handles responses sent from your server to the client
function handleServerResponse(res, component, model, submitDataUrl, returnArea) {
    if (res.action) {
        component.handleAction(res.action);
    } else {
        model.transactionNumber = res.merchantReference;
        switch (res.resultCode) {
            case "Authorised":
                var resCallServer = callServer(submitDataUrl, model);
                window.location.href = returnArea+"/Home/Result/success";
                break;
            case "Pending":
            case "Received":
                var resCallServer = callServer(submitDataUrl, model);
                window.location.href = returnArea +"/Home/Result/pending";
                break;
            case "Refused":
                window.location.href = returnArea +"/Home/Result/failed";
                break;
            default:
                window.location.href = returnArea +`/Home/Result/error?reason=${res.resultCode}`;
                break;
        }
    }
}

//initCheckout();
