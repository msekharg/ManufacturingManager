// using System.Collections.Concurrent;
//
// namespace ManufacturingManager.Web.Services
// {
//     public class CircuitUserService : ICircuitUserService
//     {
//         public ConcurrentDictionary<string, CircuitUser> Circuits { get; private set; }
//         public event EventHandler? CircuitChanged;
//         public event UserRemovedEventHandler? UserRemoved;
//
//         void OnCircuitsChanged() => CircuitChanged?.Invoke(this, EventArgs.Empty);
//
//         void OnUserRemoved(int userId)
//         {
//             var args = new UserRemovedEventArgs();
//             args.UserId = userId;
//             UserRemoved?.Invoke(this, args);
//         }
//
//         public CircuitUserService()
//         {
//             Circuits = new ConcurrentDictionary<string, CircuitUser>();
//         }
//         public bool Connect(string circuitId, CircuitUser circuitUser) // int userId)
//         {
//             //Check concurrent sessions (circuits)
//             //int numberOfConcurrent = TestRepository.NumberOfCircuitsOpenByUserId(circuitUser.UserId);
//             //if (numberOfConcurrent ==-1)
//             //{
//             //    throw new Exception("error getting # of concurrent circuits");
//             //}
//             //if (numberOfConcurrent>=3)
//             //{
//             //    return false;
//             //}
//
//             if (Circuits.ContainsKey(circuitId))
//             {
//                 Circuits[circuitId].UserId = circuitUser.UserId;
//             }
//             else
//             {
//                 //var circuitUser = new CircuitUser();
//                 //circuitUser.UserId = userId;
//                 circuitUser.CircuitId = circuitId;
//                 Circuits[circuitId] = circuitUser;
//             }
//             //Save it in local db
//             if (!TestRepository.AddCircuit(circuitId, Convert.ToInt32(circuitUser.UserId)))
//             {
//                 //save error 
//             }
//
//             OnCircuitsChanged();
//             return true;
//         }
//
//         public void Disconnect(string circuitId)
//         {
//             return;
//             var tot = Circuits.Count();
//
//             Console.WriteLine("disconnect ciruitid " + circuitId);
//             if (!TestRepository.CloseCircuit(circuitId))
//             {
//                 //save error 
//             }
//             CircuitUser circuitRemoved;
//             Circuits.TryRemove(circuitId, out circuitRemoved);
//
//             if (circuitRemoved != null)
//             {
//                 OnUserRemoved(circuitRemoved.UserId);
//                 OnCircuitsChanged();
//
//             }
//         }
//     }
// }
