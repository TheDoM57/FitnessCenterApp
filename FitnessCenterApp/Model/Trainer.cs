
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
    
public partial class Trainer
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Trainer()
    {

        this.Exercise = new HashSet<Exercise>();

        this.Requests = new HashSet<Requests>();

        this.User = new HashSet<User>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Patronymic { get; set; }

    public Nullable<int> PhotoId { get; set; }

    public int PreviousWorkLength { get; set; }

    public System.DateTime EmploymentDate { get; set; }

    public int GenderId { get; set; }

    public int SpecializationId { get; set; }

    public string WorkAchievements { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Exercise> Exercise { get; set; }

    public virtual Gender Gender { get; set; }

    public virtual Photo Photo { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Requests> Requests { get; set; }

    public virtual Specialization Specialization { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<User> User { get; set; }

}

}
