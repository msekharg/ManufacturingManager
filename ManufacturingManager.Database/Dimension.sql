

/****** Object:  Table [dbo].[Dimension]    Script Date: 6/18/2024 7:01:08 PM ******/

SET ANSI_NULLS ON

GO

 

SET QUOTED_IDENTIFIER ON

GO



CREATE TABLE [dbo].[Dimension](

    [DimensionId] [int] IDENTITY(1,1) NOT NULL,

    [InspectorName] [varchar](255) NOT NULL,

    [TubeThickness26] [float] NULL,

    [ClampThickness] [float] NULL,

    [RailTubeHeight23] [float] NULL,

    [RailTubeWidth22] [float] NULL,

    [TubeWeldSeamLocation24] [float] NULL,

    [TubeLength2] [float] NULL,

    [PaintIdentification] [int] NULL,

    [TubeCornerRadius2Clock25] [int] NULL,

    [TubeCornerRadius4Clock25] [int] NULL,

    [TubeCornerRadius8Clock25] [int] NULL,

    [TubeCornerRadius10Clock25] [int] NULL,

    [EndOfTubeCLDistance] [float] NULL,

    [TorquePerNote2] [float] NULL,

    [TorquePerNote3] [float] NULL,

    [TorqueMarking] [int] NULL,

    [TubePreGalvCoatingThickness] [float] NULL,

    [ClampPreCoatingThickness] [float] NULL,

    [TotalNoOfHolesBottom3] [int] NULL,

    [PartMarking] [int] NULL,

    [RivetPresence] [int] NULL,

    [Appearance] [int] NULL,

    [CreatedBy] [varchar](255) NOT NULL,

    [CreatedDate] [datetime] NOT NULL,

    [UpdatedBy] [varchar](255) NULL,

    [UpdatedDate] [datetime] NULL,

    PRIMARY KEY NONCLUSTERED

(

[DimensionId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

    ) ON [PRIMARY]

    GO



ALTER TABLE [dbo].[Dimension] ADD  DEFAULT (getdate()) FOR [CreatedDate]

    GO