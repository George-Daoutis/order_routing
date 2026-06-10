import { useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import './App.css';

function App() {


    return (
        <div className="flex flex-col h-screen overflow-hidden bg-slate-900 text-white">

            <header className="h-[15vh] w-full bg-slate-800 border-b border-slate-700 flex items-center px-6 shrink-0">
                <h1 className="text-xl font-bold tracking-wide">Header!</h1>
            </header>

            <main className="flex-1 overflow-y-auto p-3 space-y-3">
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">
                    <div>

                    </div>
                    Line Item 1
                </div>
                <div className="p-8 bg-slate-800/50 rounded border border-slate-700">Line Item 2</div>
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