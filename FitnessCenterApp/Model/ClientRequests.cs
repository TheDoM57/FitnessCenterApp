
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
    
public partial class ClientRequests
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ClientRequests()
    {

        this.Client = new HashSet<Client>();

        this.ClientRequests1 = new HashSet<ClientRequests>();

    }


    public int Id { get; set; }

    public int RequestId { get; set; }

    public Nullable<int> NextId { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Client> Client { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ClientRequests> ClientRequests1 { get; set; }

    public virtual ClientRequests ClientRequests2 { get; set; }

    public virtual Requests Requests { get; set; }

}

}