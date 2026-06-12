export function OrderLine() {
  return (
      <div className="flex gap-6 p-6 bg-slate-800/50 rounded border border-slate-700">
          <div className="flex flex-col space-y-2">
              <h1>Περιγραφή: </h1>
              <input className="border-b-2 border-blue-300 focus:outline-none focus:border-blue-200" placeholder="Προϊόν" />
              <h1 className="text-xs text-gray-300">#412351</h1>
          </div>

          <div className="flex flex-col space-y-2 ml-6">
              <h1>Ποσότητα: </h1>
              <input className="border-b-2 border-blue-300 w-20 focus:outline-none focus:border-blue-200" placeholder="#" />
          </div>

          <div className="flex flex-col space-y-2 ml-20">
              <h1>Κατάσταση: </h1>
              <h1 className="">Αναμονή</h1>
          </div>

          <div>

          </div>
      </div>
  );
}