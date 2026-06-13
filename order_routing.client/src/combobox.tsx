import { Combobox, ComboboxInput, ComboboxOption, ComboboxOptions } from '@headlessui/react'
import { useState } from 'react'

interface Person {
    id: number
    name: string
}

const people: Person[] = [
    { id: 1, name: 'Προϊόν Α' },
    { id: 2, name: 'Προϊόν Β' },
    { id: 3, name: 'Προϊόν Γ' },
]

export function ComboBox() {
    const [selectedPerson, setSelectedPerson] = useState<Person | null>(null)
    const [query, setQuery] = useState<string>('')

    const filteredPeople: Person[] =
        query === ''
            ? people
            : people.filter((person: Person) => {
                return person.name.toLowerCase().includes(query.toLowerCase())
            })

    return (
        <Combobox value={selectedPerson} onChange={setSelectedPerson} onClose={() => setQuery('')}>
            <div className="relative">
                <ComboboxInput
                    aria-label="Assignee"
                    className="w-64 bg-slate-900 text-white border-b-2 border-blue-300 pb-1 text-sm focus:outline-none focus:border-blue-200 transition-colors"
                    displayValue={(person: Person | null) => person?.name ?? ''}
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => setQuery(event.target.value)}
                    placeholder="Επιλέξτε προϊόν..."
                />

                <ComboboxOptions
                    anchor="bottom start"
                    className="w-64 mt-1 max-h-60 overflow-auto rounded border border-slate-700 bg-slate-800 p-1 text-sm text-white shadow-xl focus:outline-none empty:invisible z-50 [--anchor-gap:4px]"
                >
                    {filteredPeople.length === 0 && query !== '' ? (
                        <div className="relative cursor-default select-none py-2 px-3 text-slate-400">
                            Δεν βρέθηκαν αποτελέσματα.
                        </div>
                    ) : (
                        filteredPeople.map((person: Person) => (
                            <ComboboxOption
                                key={person.id}
                                value={person}
                                className="relative cursor-pointer select-none rounded py-2 px-3 text-slate-200 data-[focus]:bg-slate-700 data-[focus]:text-white transition-colors"
                            >
                                {person.name}
                            </ComboboxOption>
                        ))
                    )}
                </ComboboxOptions>
            </div>
        </Combobox>
    )
}