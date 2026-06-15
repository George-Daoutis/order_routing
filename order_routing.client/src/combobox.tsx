import { Combobox, ComboboxInput, ComboboxOption, ComboboxOptions } from '@headlessui/react'
import { useState } from 'react'

interface ProductLookup {
    id: number;
    productCode: string;
    description: string;
}

interface ComboBoxProps {
    products: Map<number, ProductLookup>;
}

export function ComboBox( productsLookup : ComboBoxProps) {
    const [selectedItem, setSelectedItem] = useState<ProductLookup | null>(null);
    const [query, setQuery] = useState<string>('');

    const productArray = Array.from(productsLookup.products || [], ([key, value]) => ({
        id: key,
        ...value
    }));

    const filteredProducts: ProductLookup[] =
        query === ''
            ? productArray
            : productArray.filter((product: ProductLookup) => {
                return product.description.toLowerCase().includes(query.toLowerCase());
            });

    return (
        <Combobox value={selectedItem} onChange={setSelectedItem} onClose={() => setQuery('')}>
            <div className="relative">
                <ComboboxInput
                    aria-label="Assignee"
                    className="w-64 bg-slate-900/60 text-white font-semibold border-b-2 border-blue-300 py-0.5 text-sm focus:outline-none focus:border-blue-200 transition-colors placeholder:text-slate-500"
                    displayValue={(product: ProductLookup | null) => product?.description ?? ''}
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => setQuery(event.target.value)}
                    placeholder="Επιλέξτε προϊόν..."
                />

                <ComboboxOptions
                    anchor="bottom start"
                    className="w-64 mt-1 max-h-60 overflow-auto rounded border border-slate-700/60 bg-slate-900 p-1 text-xs text-slate-200 shadow-2xl focus:outline-none empty:invisible z-50 [--anchor-gap:4px] scrollbar-thin scrollbar-thumb-slate-700"
                >
                    {filteredProducts.length === 0 && query !== '' ? (
                        <div className="relative cursor-default select-none py-2 px-3 text-slate-500">
                            Δεν βρέθηκαν αποτελέσματα.
                        </div>
                    ) : (
                        filteredProducts.map((product: ProductLookup) => (
                            <ComboboxOption
                                key={product.id}
                                value={product}
                                className="relative cursor-pointer select-none rounded py-1.5 px-3 text-slate-300 data-[focus]:bg-slate-800 data-[focus]:text-white transition-colors"
                            >
                                {product.description}
                            </ComboboxOption>
                        ))
                    )}
                </ComboboxOptions>
            </div>
        </Combobox>
    );
}