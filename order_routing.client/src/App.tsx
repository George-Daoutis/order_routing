import { useMemo, useEffect, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
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
    const queryClient = useQueryClient();
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [errorMessage, setErrorMessage] = useState<string>("");
    const [showPassword, setShowPassword] = useState<boolean>(false);

    const [currentUser, setCurrentUser] = useState<User | null>(() => {
        const savedUser = localStorage.getItem("user");
        const token = localStorage.getItem("token");

        if (savedUser && token) {
            try {
                return JSON.parse(savedUser) as User;
            } catch (error) {
                localStorage.removeItem("user");
                localStorage.removeItem("token");
                console.log("Local User Parse Error: ", error.message);
                return null;
            }
        }
        return null;
    });

    interface userResponse {
        username: string;
        token: string;
        role: string;
        storeId: number | null;
    }

    interface User {
        username: string;
        role: string;
        storeId: number | null;
    }

    const handleLogin = async () => {
        if (!username.trim() || !password.trim()) {
            setErrorMessage("Όνομα Χρήστη και κωδικός δεν μπορούν να είναι κενά.");
            return;
        }
        const payload = {
            Username: username,
            Password: password
        }
        try {
            setErrorMessage("");
            const response = await fetch('/api/auth/login', {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(payload)
            });
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || "Login failed");
            }

            const data: userResponse = await response.json();

            localStorage.setItem("token", data.token)
            localStorage.setItem("user", JSON.stringify({
                username: data.username,
                role: data.role,
                storeId: data.storeId
            }));

            console.log("Logged in successfully");
            setUsername("");
            setPassword("");
            setCurrentUser({ username: data.username, role: data.role, storeId: data.storeId });

            await queryClient.invalidateQueries();
        }
        catch (error) {
            console.error("Error during login:", error.message);
            setErrorMessage(error.message);
        }

    }

    const handleLogout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        queryClient.clear();
        setCurrentUser(null);
    }





    const { data: currentStore = [] } = useQuery<StoreLookup>({
        queryKey: ['currentStore'],
        queryFn: async () => {
            const token = localStorage.getItem('token')
            const response = await fetch('/api/store/current', {
                method: "GET",
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        },
        enabled: !!currentUser
    })

    const { data: products = [] } = useQuery<ProductLookup[]>({
        queryKey: ['products'],
        queryFn: async () => {
            const token = localStorage.getItem('token')
            const response = await fetch('/api/orderline/getproducts', {
                method: "GET",
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        },
        enabled: !!currentUser
    })

    const { data: stores = [] } = useQuery<StoreLookup[]>({
        queryKey: ['stores'],
        queryFn: async () => {
            const token = localStorage.getItem('token')
            const response = await fetch('/api/store', {
                method: "GET",
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        },
        enabled: !!currentUser
    })

    const { data: orders } = useQuery({
        queryKey: ["orders"],
        queryFn: async () => {
            const token = localStorage.getItem('token')
            const response = await fetch('/api/orderline', {
                method: "GET",
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) throw new Error("Network Error");
            return response.json();
        },
        enabled: !!currentUser
    })




    const productLookup = useMemo(() => {
        return new Map<number, ProductLookup>(products.map(p => [p.id, p]));
    }, [products]);

    const storesLookup = useMemo(() => {
        return new Map<number, StoreLookup>(stores.map(s => [s.id, s]));
    }, [stores]);





    
    {/* Γιατί έτσι μας αρέσει :) */}
    const currentHour = new Date().getHours();
    const greeting = currentHour >= 12 ? "Καλησπέρα" : "Καλημέρα";

    return (
        <div className="flex flex-col h-screen overflow-hidden bg-slate-900 text-white">


            <header className="h-[15vh] w-full bg-slate-800 border-b border-slate-700 flex items-center px-6 shrink-0 gap-4">
                {/* Title */}
                <h1 className="text-xl font-bold tracking-wide">
                    {currentUser ? `${greeting} ${currentUser.username}!` : `${greeting}`}
                </h1>

                {localStorage.getItem("token") ? (
                    <button
                        className="bg-blue-600 hover:bg-blue-700 px-4 py-1 rounded font-medium transition-colors ml-auto"
                        onClick={handleLogout}
                    >
                        Log Out
                    </button>
                ) : (
                    <div className="flex flex-col items-end justify-center h-12 ml-auto">

                        {/* Input fields */}
                        <div className="flex items-center gap-4">
                            <input
                                type="text"
                                value={username}
                                className="bg-slate-700 text-white px-3 py-1 rounded border border-slate-600 focus:outline-none w-48"
                                placeholder="Όνομα Χρήστη"
                                onChange={(e) => setUsername(e.target.value)}
                            />

                            <div className="relative flex items-center">
                                <input
                                    type={showPassword ? "text" : "password"}
                                    value={password}
                                    className="bg-slate-700 text-white pl-3 pr-10 py-1 rounded border border-slate-600 focus:outline-none w-48"
                                    placeholder="Κωδικός"
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                                <button
                                    type="button"
                                    className="absolute right-2 text-slate-400 hover:text-slate-200 focus:outline-none flex items-center"
                                    onClick={() => setShowPassword(!showPassword)}
                                >
                                    {showPassword ? (
                                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5">
                                            <path strokeLinecap="round" strokeLinejoin="round" d="M3.98 8.223A10.477 10.477 0 0 0 1.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.451 10.451 0 0 1 12 4.5c4.756 0 8.773 3.162 10.065 7.498a10.522 10.522 0 0 1-4.293 5.774M6.228 6.228 3 3m3.228 3.228 3.65 3.65m7.894 7.894L21 21m-3.228-3.228-3.65-3.65m0 0a3 3 0 1 0-4.243-4.243m4.242 4.242L9.88 9.88" />
                                        </svg>
                                    ) : (
                                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5">
                                            <path strokeLinecap="round" strokeLinejoin="round" d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z" />
                                            <path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
                                        </svg>
                                    )}
                                </button>
                            </div>

                                {/* Login button */}
                            <button
                                className="bg-blue-600 hover:bg-blue-700 px-4 py-1 rounded font-medium transition-colors shrink-0"
                                onClick={handleLogin}
                            >
                                Log In
                            </button>
                        </div>

                        <span
                            className={`text-red-400 text-xs font-medium tracking-wide mr-1 h-4 block py-2 transition-opacity duration-200 ${errorMessage ? 'opacity-100' : 'opacity-0 select-none'
                                }`}
                        >
                            {errorMessage || "\u00A0"}
                        </span>
                    </div>
                )}
            </header>

            <main className="flex-1 overflow-y-auto p-3 space-y-3">
                {orders?.map((order: Order) => (
                    <OrderLine key={order.id} products={productLookup} stores={storesLookup} currentStore={currentStore} order={order} />
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