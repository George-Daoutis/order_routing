import { useEffect, useState } from 'react';
import './App.css';

function App() {


    return (
        <div className="flex flex-col h-screen overflow-hidden bg-slate-900 text-white">

            <header className="h-[15vh] w-full bg-slate-800 border-b border-slate-700 flex items-center px-6 shrink-0">
                <h1 className="text-xl font-bold tracking-wide">Application Header (15%)</h1>
            </header>

            <main className="flex-1 overflow-y-auto p-6 space-y-4">
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 1</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 2</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 3</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 4</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 5</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 6</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 7</div>
                <div className="p-4 bg-slate-800/50 rounded border border-slate-700">Line Item 8</div>
            </main>

            <footer className="h-[10vh] w-full bg-slate-950 border-t border-slate-800 flex items-center px-6 shrink-0">
                <p className="text-sm text-slate-400">Application Footer (10%)</p>
            </footer>

        </div>

    );
}

export default App;