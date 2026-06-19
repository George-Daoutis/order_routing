import { ComboBox } from './combobox.tsx';
import { useState, useEffect } from 'react';

export enum ShippingMethod {
    Standard = "Standard Delivery",
    Express = "Express Courier",
    StorePickup = "Store Pickup"
}

// 2. Το Interface για τα δεδομένα της γραμμής
interface FulfillmentItem {
    id: number;
    storeName: string;
    quantity: number;
    transportMethod: string;
    isStepOneConfirmed: boolean;
    isStepTwoConfirmed: boolean;
}
interface StoreLookup {
    id: number;
    storeDescription: string;
    address: string;
    phoneNumber: string;
}

interface ProductLookup {
    id: number;
    productCode: string;
    description: string;
}

export function OrderLine({ products, stores, currentStore, order }) {
    console.log(order);
    const [items, setItems] = useState<FulfillmentItem[]>([]);
    const [orderItem, setOrderItem] = useState<ProductLookup>();
    const [isConfirmed, setIsConfirmed] = useState<boolean>(false);

    const handleQuantityChange = (id: number, value: string) => {
        const parsedValue = parseInt(value, 10) || 0;
        
        setItems(prev => prev.map(item => 
            item.id === id ? { ...item, quantity: parsedValue } : item
        ));
    };

    const OrderStatusLabels: Record<string, string> = {
        "Requested": "Αναμονή",
        "AwaitForConfirmation": "Αναμονή για Επιβεβαίωση",
        "PartiallyFulfilled": "Μερικώς Συμπληρωμήνη",
        "Completed": "Ολοκληρώθηκε"
    };

    const TransportMethods: Record<string, string> = {
        "Requested": "Αναμονή",
        "AwaitForConfirmation": "Αναμονή για Επιβεβαίωση",
        "PartiallyFulfilled": "Μερικώς Συμπληρωμήνη",
        "CompanyDriver": "Ολοκληρώθηκε"
    };

    const handleShippingChange = (id: number, method: string) => {
        setItems(prev => prev.map(item => item.id === id ? { ...item, transportMethod: method } : item));
    };

    const handleConfirmStepOne = (id: number) => {
        setItems(prev => prev.map(item => item.id === id ? { ...item, isStepOneConfirmed: true } : item));
    };

    const handleConfirmStepTwo = (id: number) => {
        setItems(prev => prev.map(item => item.id === id ? { ...item, isStepTwoConfirmed: true } : item));
    };

    useEffect(() => {
        const orderFill = () => {
            if (order && orderItem == null) {
                const prod = products.get(order.productId);
                setOrderItem(prod);
                setIsConfirmed(true);
                setItems(order.orderLineFulfillment.map((fulfillments) => {
                    const fulfillmentStore = stores.get(fulfillments.storeId);
                    return {
                        id: fulfillments.storeId,
                        storeName: fulfillmentStore ? fulfillmentStore.storeDescription: "Unknown Store",
                        quantity: fulfillments.quantity,
                        transportMethod: fulfillments.fulfillmentTransportMethod,
                        isStepOneConfirmed: false,
                        isStepTwoConfirmed: false
                    }
                }));
            }
        };

        orderFill();
    });

    const handleAddFulfillment = () => {
        const newFulfillment: FulfillmentItem = { id: currentStore.id, storeName: currentStore.storeDescription, quantity: 0, transportMethod: ShippingMethod.Standard, isStepOneConfirmed: false, isStepTwoConfirmed: false };
        setItems((prevItems) => [...prevItems, newFulfillment]);
    };

        return (
            <div className="flex p-4 bg-slate-800/50 rounded border border-slate-700 items-start">

                {/* Περιγραφή & ID */}
                <div className="flex flex-col space-y-1 pt-2">
                    <span className="text-xs uppercase tracking-wider text-slate-300 font-semibold">Περιγραφή</span>
                    <ComboBox productsLookup={products} value={orderItem?.description} />
                    <span className="text-[14px] text-slate-400">{orderItem?.productCode}</span>
                </div>

                {/* Ποσότητα Input */}
                <div className="flex flex-col space-y-1 pt-2 ml-8">
                    <span className="text-xs uppercase tracking-wider text-slate-300 font-semibold text-center">Ποσότητα</span>
                    <input
                        disabled={isConfirmed }
                        type="number"
                        min="0"
                        onKeyDown={(e) => {
                            if (["e", "E", "+", "-", ".", ","].includes(e.key)) e.preventDefault();
                        }}
                        className="border-b-2 bg-slate-900/60 border-blue-300 w-16 focus:outline-none focus:border-blue-200 font-mono text-sm font-bold text-blue-400 text-center py-0.5 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
                        placeholder="#"
                        defaultValue={order?.amount}
                        />
                </div>

                {/* Κατάσταση */}
                <div className="flex flex-col space-y-1 pt-2 ml-12">
                    <span className="text-xs uppercase tracking-wider text-slate-300 font-semibold">Κατάσταση</span>
                    <span className="text-sm font-semibold text-white py-0.5">{OrderStatusLabels[order?.orderLineStatus]}</span>
                </div>

                {/* Fulfillment Λίστα */}
                <div className="ml-16 mr-16">
                    <div className="bg-transparent flex flex-col text-slate-100 w-full">

                        {/* COLUMN HEADERS */}
                        <div className="flex gap-4 px-3 py-2 text-xs uppercase tracking-wider text-slate-300 font-semibold border-b border-slate-700/50 mb-2">
                            <div className="w-52">Μαγαζί</div>
                            <div className="w-24 text-center">Ποσότητα</div>
                            <div className="w-24">Έλεγχος</div>
                            <div className="w-44">Αποστολή</div>
                            <div className="w-28 pl-1">Κατάσταση</div>
                        </div>

                        <div className="space-y-1.5 max-h-[168px] overflow-y-auto pr-1 scrollbar-thin scrollbar-thumb-slate-700">
                            {items.map((item) => (
                                <div
                                    key={item.id}
                                    className="flex gap-4 p-2.5 bg-slate-800/50 rounded border border-slate-700 items-center text-xs"
                                >
                                    {/* Όνομα Μαγαζιού */}
                                    <div className="flex flex-col w-52 truncate">
                                        <span className="text-sm font-semibold text-white truncate">{item?.storeName}</span>
                                    </div>

                                    {/* Input Ποσότητας */}
                                    <div className="w-24 flex justify-center">
                                        <input
                                            type="number"
                                            min="0"
                                            value={item.quantity === 0 ? "" : item.quantity}
                                            onChange={(e) => handleQuantityChange(item.id, e.target.value)}
                                            onKeyDown={(e) => {
                                                if (["e", "E", "+", "-", ".", ","].includes(e.key)) e.preventDefault();
                                            }}
                                            disabled={item.isStepTwoConfirmed}
                                            className="border-b-2 bg-slate-900/60 border-blue-300 w-16 focus:outline-none focus:border-blue-200 font-mono text-sm font-bold text-blue-400 text-center py-0.5 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none disabled:border-slate-700 disabled:text-slate-500 disabled:cursor-not-allowed"
                                            placeholder="#"
                                        />
                                    </div>

                                    {/* Κουμπί Verify */}
                                    <div className="w-24">
                                        <button
                                            onClick={() => handleConfirmStepOne(item.id)}
                                            disabled={!item.quantity || item.quantity <= 0 || item.isStepTwoConfirmed}
                                            className={`w-20 py-1 text-[11px] font-medium rounded border transition-all ${item.isStepOneConfirmed
                                                    ? "bg-emerald-950/40 border-emerald-600 text-emerald-400"
                                                    : "bg-slate-900/60 border-slate-600 text-slate-300 hover:bg-slate-800"
                                                } disabled:bg-slate-800/40 disabled:border-slate-700 disabled:text-slate-600 disabled:cursor-not-allowed`}
                                        >
                                            {item.isStepOneConfirmed ? "✓ OK" : "Verify"}
                                        </button>
                                    </div>

                                    {/* Dropdown Αποστολής */}
                                    <div className="w-44">
                                        <select
                                            value={TransportMethods[item.transportMethod]}
                                            onChange={() => handleShippingChange(item.id, TransportMethods[item.transportMethod])}
                                            disabled={item.isStepTwoConfirmed}
                                            className="border-b bg-slate-900/60 border-blue-300 focus:outline-none focus:border-blue-200 py-0.5 text-white cursor-pointer w-40 disabled:border-slate-700 disabled:text-slate-500 disabled:cursor-not-allowed text-ellipsis"
                                        >
                                            {Object.values(TransportMethods).map((method) => (
                                                <option key={method} value={method} className="bg-slate-900 text-white text-xs">
                                                    {method}
                                                </option>
                                            ))}
                                        </select>
                                    </div>

                                    {/* Κουμπί Fulfill */}
                                    <div className="w-28 pl-1">
                                        <button
                                            onClick={() => handleConfirmStepTwo(item.id)}
                                            disabled={!item.isStepOneConfirmed}
                                            className={`px-4 py-1 text-[11px] font-medium rounded border transition-all ${item.isStepTwoConfirmed
                                                    ? "bg-blue-600 border-blue-500 text-white"
                                                    : !item.isStepOneConfirmed
                                                        ? "bg-slate-800 text-slate-600 border-slate-700 cursor-not-allowed"
                                                        : "bg-blue-950/40 border-blue-600 text-blue-400 hover:bg-blue-900/50"
                                                }`}
                                        >
                                            {item.isStepTwoConfirmed ? "⚡ Routed" : "Fulfill"}
                                        </button>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
                <button onClick={() => handleAddFulfillment()} disabled={items.some(i => i.id == currentStore.id)} >test</button>



            </div>
        );
    }

