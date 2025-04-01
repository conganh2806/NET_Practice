// using System;
// using System.Linq;
// using System.Collections.Generic;

// namespace Excercise1
// {
//     public enum LetterType
//     {
//         FAST,
//         NONE,
//     }

//     public interface IShippingFee 
//     {
//         int GetShippingFee();
//     }

//     public class Recipient
//     {
//         public string Name { get; set; }
//         public string Address { get; set; }

//         public override string ToString() 
//         { 
//             return $"name: {Name}, address: {Address}";
//         }
//     }

//     public abstract class Parcel
//     {
//         private Recipient _recipient = new Recipient();
//         public Recipient Recipient 
//         { 
//             get => _recipient; 
//             set => _recipient = value; 
//         }
//         public Parcel(string address, string recipientName)
//         {
//             _recipient.Name = recipientName;
//             _recipient.Address = address;
//         }
//         public override string ToString()
//         {
//             return $"Recipient: {_recipient}";
//         }
//     }

//     public class Letter : Parcel, IShippingFee
//     {
//         public LetterType Type { get; set; }

//         public Letter(string address, string recipientName, LetterType type)
//             : base(address, recipientName)
//         {
//             Type = type;
//         }

//         public int GetShippingFee()
//         {
//             return Type == LetterType.FAST ? 3000 : 500;
//         }

//         public override string ToString()
//         {
//             return $"Letter - Recipient: {this.Recipient}, Type: {this.Type}, Cost: {GetShippingFee()}";
//         }
//     }

//     public class Goods : Parcel, IShippingFee
//     {
//         public int Weight { get; set; }

//         public Goods(string address, string recipientName, int weight)
//             : base(address, recipientName)
//         {
//             Weight = weight;
//         }

//         public int GetShippingFee()
//         {
//             return 10000 * Weight;
//         }

//         public override string ToString()
//         {
//             return $"Goods - Recipient: {this.Recipient}, Weight: {this.Weight}kg, Cost: {GetShippingFee()}";
//         }
//     }

//     public class PostOffice : IEnumerable<Parcel>
//     { 
//         private List<Parcel> _parcels = new List<Parcel>();
       
//         public void InitializeParcels() 
//         { 
//             _parcels.Add(new Goods("K351/08 Hải Phòng", "Nguyễn Văn A", 50));
//             _parcels.Add(new Letter("Hà Nội", "Lê Văn C", LetterType.FAST));
//             _parcels.Add(new Goods("33 Lê Duẩn", "Trần Văn B", 50));
//             _parcels.Add(new Letter("Cà Mau", "Lê Văn D", LetterType.NONE));
//         }

//         public void AddNewParcel(Parcel parcel)
//         { 
//             if(parcel == null)
//             {
//                 Console.WriteLine("Cannot add parcel! Parcel is null..");
//             }
//             else
//             { 
//                 _parcels.Add(parcel);
//             }
//         }

//         public int GetGoodsCount() 
//         { 
//             //return _parcels.OfType<Goods>.Count();
//             return _parcels.Count(p => p is Goods);
//         }

//         public List<Parcel> ExtractLettersRelevantTo(string name) 
//         { 
//             return _parcels.Where(p => p.Recipient.Name == name).ToList();
//         }

//         public List<Parcel> SortParcels() 
//         { 
//             var res = _parcels.OrderBy(p => p.Recipient.Name)
//                             .ThenBy(p => (p as IShippingFee)?.GetShippingFee() ?? 0)
//                             .ToList();
//             return res;
//         }

//         public void RemoveRegularLetter() 
//         { 
//             _parcels.RemoveAll(p => p is Letter letter && letter.Type == LetterType.NONE);
//         }

//         public int TotalShippingFee() 
//         { 
//             return _parcels.Sum(p => (p as IShippingFee)?.GetShippingFee() ?? 0);
//         }


//         public IEnumerator<Parcel> GetEnumerator() 
//         {
//             return _parcels.GetEnumerator();
//         }

//         System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//         {
//             return GetEnumerator();
//         }

//         public void PrintAllParcel()
//         {
//             _parcels.ForEach(parcel =>
//             {
//                 if (parcel is IShippingFee shippingFeeParcel)
//                 {
//                     Console.WriteLine($"{parcel} | Shipping fee: {shippingFeeParcel.GetShippingFee()}");
//                 }
//                 else
//                 {
//                     Console.WriteLine($"{parcel} | No shipping fee available");
//                 }
//             });
//         }

//         public void PrintParcels(List<Parcel> parcels)
//         {
//             parcels.ForEach(parcel => Console.WriteLine(parcel));
//         }
//     }

//     class Program
//     {
//         static void Main(string[] args)
//         {
//             PostOffice p1 = new PostOffice();
//             p1.InitializeParcels();
//             p1.AddNewParcel(new Letter("Hà Giang", "Nguyễn Văn A", LetterType.FAST));
//             p1.AddNewParcel(new Goods("Cà Mau", "Lê Văn C", 40));
//             var res = p1.ExtractLettersRelevantTo("Nguyễn Văn A");
//             // p1.PrintParcels(res);
//             p1.SortParcels();
//             // p1.PrintAllParcel();
//             p1.RemoveRegularLetter();
//             p1.PrintAllParcel();
//             Console.WriteLine(p1.TotalShippingFee());

//         }
//     }
// }
