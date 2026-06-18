import { useMemo, useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import './App.css';
import { OrderLine } from './OrderLine.tsx'

interface ProductLookup {
    id: number;
    productCode: string;
    description: string;
}

interface StoreLookup {
    id: number;
    storeDescription: string;
    address: string;
    phoneNumber: string;
}

interface Order {
    id: number;
}


function App() {
    const { data: currentStore = [] } = useQuery<StoreLookup>({
        queryKey: ['currentStore'], queryFn: async () => {
            const response = await fetch('/api/store/current', {
                method: "GET",
                credentials: "include"
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        }
    })

    const { data: products = [] } = useQuery<ProductLookup[]>({
        queryKey: ['products'], queryFn: async () => {
            const response = await fetch('/api/orderline/getproducts', {
                method: "GET",
                credentials: "include"
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        }
    })

    const { data: stores = [] } = useQuery<StoreLookup[]>({
        queryKey: ['stores'], queryFn: async () => {
            const response = await fetch('/api/store', {
                method: "GET",
                credentials: "include"
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        }
    })

    const { data: orders } = useQuery({
        queryKey: ["orders"],
        queryFn: async () => {
            const response = await fetch('/api/orderline', {
                method: "GET",
                credentials: "include"
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        }
    })

    const productLookup = useMemo(() => {
        return new Map<number, ProductLookup>(products.map(p => [p.id, p]));
    }, [products]);

    const storesLookup = useMemo(() => {
        return new Map<number, StoreLookup>(stores.map(s => [s.id, s]));
    }, [stores]);
    

    return (
        <div className="flex flex-col h-screen overflow-hidden bg-slate-900 text-white">

            <header className="h-[15vh] w-full bg-slate-800 border-b border-slate-700 flex items-center px-6 shrink-0">
                <h1 className="text-xl font-bold tracking-wide">Header!</h1>
            </header>

            <main className="flex-1 overflow-y-auto p-3 space-y-3">
                {orders?.map((order: Order) => (
                    <OrderLine key={order.id} products={productLookup} stores={stores} currentStore={currentStore} order={order} />
                ))}
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 3</div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 4</div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 5</div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 6</div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 7</div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 8</div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 9</div>
            </main>

            <footer className="h-[8vh] w-full bg-slate-950 border-t border-slate-800 flex items-center px-6 shrink-0">
                <p className="text-sm text-slate-400">Footer!</p>
            </footer>

        </div>

    );
}

export default App;