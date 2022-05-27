
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace FitnessCenterApp.Model
{

using System;
    using System.Collections.Generic;
    
public partial class Client
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Client()
    {

        this.Requests = new HashSet<Requests>();

        this.Users = new HashSet<Users>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public System.DateTime BirthDate { get; set; }

    public int GenderId { get; set; }

    public Nullable<int> RequestPtrId { get; set; }



    public virtual ClientRequests ClientRequests { get; set; }

    public virtual Gender Gender { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Requests> Requests { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Users> Users { get; set; }

}

}
